using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public int ControllerId;
    public ControllersManager ControllerManagerEntity;
    public float JumpStrenght;
    public float ForwardSpeed;

    private const int JumpCountLimit = 2;
    private Rigidbody2D _rigidbody2D = null;
    private bool _isGrounded = false;
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
        if (this._isGrounded || this._jumpCount < JumpCountLimit - 1)
        {

            if(this.JumpButton(this.ControllerId, true))
            {
                Debug.Log("Jump");
                jumped = true;
                this._jumpCount += 1;
            }
            _rigidbody2D.velocity = new Vector2(((jumped)?this.WalkDirection(this.ControllerId).x * this.ForwardSpeed /2 : this._rigidbody2D.velocity.x), _rigidbody2D.velocity.y + ((jumped)? this.JumpStrenght : 0));
        }

        //Walk
        Vector2 walkDirection = new Vector2(this.WalkDirection(this.ControllerId).x * this.ForwardSpeed, 0);
        if (this._isGrounded)
        {
            _rigidbody2D.velocity = new Vector2(this.WalkDirection(this.ControllerId).x * this.ForwardSpeed, _rigidbody2D.velocity.y + ((jumped) ? this.JumpStrenght : 0));
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8)
        {
            this._isGrounded = true;
            this._jumpCount = 0;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8)
        {
            this._isGrounded = false;
            this._jumpCount = 0;
        }
    }

    private void DebugMovement()
    {
        //Sticks
        Debug.Log(string.Format("L-A {0}", this.ControllerManagerEntity.GetLeftAnalog(this.ControllerId)));
        Debug.Log(string.Format("R-A {0}", this.ControllerManagerEntity.GetRightAnalog(this.ControllerId)));
        Debug.Log(string.Format("HAT {0}", this.ControllerManagerEntity.GetHat(this.ControllerId)));
        //AXIS
        Debug.Log(string.Format("L-T {0}", this.ControllerManagerEntity.GetLeftTrigger(this.ControllerId)));
        Debug.Log(string.Format("R-T {0}", this.ControllerManagerEntity.GetRightTrigger(this.ControllerId)));
        //Buttons
        Debug.Log(string.Format("B-B {0}", this.ControllerManagerEntity.GetButton0(this.ControllerId)));
        Debug.Log(string.Format("A-B {0}", this.ControllerManagerEntity.GetButton1(this.ControllerId)));
        Debug.Log(string.Format("X-B {0}", this.ControllerManagerEntity.GetButton2(this.ControllerId)));
        Debug.Log(string.Format("Y-B {0}", this.ControllerManagerEntity.GetButton3(this.ControllerId)));
        Debug.Log(string.Format("L-S {0}", this.ControllerManagerEntity.GetButtonLeft(this.ControllerId)));
        Debug.Log(string.Format("R-S {0}", this.ControllerManagerEntity.GetButtonRight(this.ControllerId)));
        Debug.Log(string.Format("H-B {0}", this.ControllerManagerEntity.GetButtonHome(this.ControllerId)));
        Debug.Log(string.Format("S-B {0}", this.ControllerManagerEntity.GetButtonStart(this.ControllerId)));
        Debug.Log(string.Format("ASB {0}", this.ControllerManagerEntity.GetButtonAltStart(this.ControllerId)));
    }
}
