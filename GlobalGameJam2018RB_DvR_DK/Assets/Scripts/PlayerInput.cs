using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public int ControllerId;
    public ControllersManager ControllerManagerEntity;
    public LookDirection LookDirection;
    public float JumpStrenght;
    public float ForwardSpeed;
    public BoxCollider2D BodyCollider2D;

    private const int JumpCountLimit = 2;
    private Rigidbody2D _rigidbody2D = null;
    private bool _isGrounded = false;
    private bool _isWalled = false;
    private Transform _lastWallHit = null;
    private int _jumpCount = 0;
    

    delegate bool ControllerBooleanMethod(int controllerId, bool mustRelease = false);
    delegate Vector2 ControllerAnalogMethod(int controllerId);

    private ControllerBooleanMethod JumpButton
    {
        get { return this.ControllerManagerEntity.GetButton1; }
    }
    private ControllerBooleanMethod FireButton
    {
        get { return this.ControllerManagerEntity.GetButtonRight; }
    }
    private ControllerAnalogMethod WalkDirection
    {
        get { return this.ControllerManagerEntity.GetLeftAnalog; }
    }
    private ControllerAnalogMethod AimDirection
    {
        get { return this.ControllerManagerEntity.GetRightAnalog; }
    }

    public PlayerInput()
    {
    }

    void Start()
    {
        this._rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (this.ControllerManagerEntity == null ||
            !this.ControllerManagerEntity.IsControllerActive(ControllerId) ||
            this._rigidbody2D == null)
        {
            return;
        }

        //jump
        bool jumped = false;

        if (this._isWalled)
        {
            if (this.JumpButton(this.ControllerId, true))
            {
                float jumpOffDirection = 0;
                jumped = true;
                this._jumpCount += 1;
                jumpOffDirection = (this._lastWallHit.position.x > this.transform.position.x) ? -1f : 1f;
                _rigidbody2D.velocity = new Vector2(((jumped) ? jumpOffDirection * this.ForwardSpeed / 2 : this._rigidbody2D.velocity.x), _rigidbody2D.velocity.y + ((jumped) ? this.JumpStrenght : 0));
            }
        }
        else if (this._isGrounded || this._jumpCount < JumpCountLimit - 1)
        {

            if(this.JumpButton(this.ControllerId, true))
            {
                jumped = true;
                this._jumpCount += 1;
            }
            _rigidbody2D.velocity = new Vector2(((jumped)?this.WalkDirection(this.ControllerId).x * this.ForwardSpeed /2 : this._rigidbody2D.velocity.x), _rigidbody2D.velocity.y + ((jumped)? this.JumpStrenght : 0));
        }


        //Walk
        if (this._isGrounded)
        {
            _rigidbody2D.velocity = new Vector2(this.WalkDirection(this.ControllerId).x * this.ForwardSpeed, _rigidbody2D.velocity.y + ((jumped) ? this.JumpStrenght : 0));
        }

        //LookDirection
        if (this.WalkDirection(this.ControllerId).x > 0)
        {
            LookDirection.SetTurnLeft();
        }
        else if(this.WalkDirection(this.ControllerId).x < 0)
        {
            LookDirection.SetTurnRight();
        }
        else
        {
            LookDirection.SetNeutral();
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8)
        {
            this._isGrounded = true;
            this._jumpCount = 0;
        }

        if (coll.gameObject.layer == 9)
        {
            this._isWalled = true;
            this._jumpCount = 0;
            this._lastWallHit = coll.gameObject.transform;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8)
        {
            this._isGrounded = false;
            this._jumpCount = 0;
        }

        if (coll.gameObject.layer == 9)
        {
            this._isWalled = false;
            this._jumpCount = 0;
            this._lastWallHit = coll.gameObject.transform;
        }
    }

    private void DebugMovement()
    {
        //Sticks
        Debug.Log(string.Format("L-A {0}", this.ControllerManagerEntity.GetLeftAnalog(3)));
        Debug.Log(string.Format("R-A {0}", this.ControllerManagerEntity.GetRightAnalog(3)));
        Debug.Log(string.Format("HAT {0}", this.ControllerManagerEntity.GetHat(3)));
        //AXIS
        Debug.Log(string.Format("L-T {0}", this.ControllerManagerEntity.GetLeftTrigger(3)));
        Debug.Log(string.Format("R-T {0}", this.ControllerManagerEntity.GetRightTrigger(3)));
        //Buttons
        Debug.Log(string.Format("B-B {0}", this.ControllerManagerEntity.GetButton0(3)));
        Debug.Log(string.Format("A-B {0}", this.ControllerManagerEntity.GetButton1(3)));
        Debug.Log(string.Format("X-B {0}", this.ControllerManagerEntity.GetButton2(3)));
        Debug.Log(string.Format("Y-B {0}", this.ControllerManagerEntity.GetButton3(3)));
        Debug.Log(string.Format("L-S {0}", this.ControllerManagerEntity.GetButtonLeft(3)));
        Debug.Log(string.Format("R-S {0}", this.ControllerManagerEntity.GetButtonRight(3)));
        Debug.Log(string.Format("H-B {0}", this.ControllerManagerEntity.GetButtonHome(3)));
        Debug.Log(string.Format("S-B {0}", this.ControllerManagerEntity.GetButtonStart(3)));
        Debug.Log(string.Format("ASB {0}", this.ControllerManagerEntity.GetButtonAltStart(3)));
    }
}
