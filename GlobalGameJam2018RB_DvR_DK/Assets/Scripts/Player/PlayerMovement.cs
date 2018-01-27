using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Parameters
    public LookDirection LookDirection;
    public float JumpStrenght;
    public float ForwardSpeed;

    //Private members
    private const int JumpCountLimit = 2;
    private Rigidbody2D _rigidbody2D = null;
    private bool _isGrounded = false;
    private bool _isWalled = false;
    private Transform _lastWallHit = null;
    private int _jumpCount = 0;

    PlayerInput pInput;

    void Start()
    {
        this._rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        pInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!pInput.IsActive())
            return;

        //jump
        bool jumped = false;

        if (this._isWalled)
        {
            if (pInput.IsJumping(true))
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

            if (pInput.IsJumping(true))
            {
                jumped = true;
                this._jumpCount += 1;
            }
            _rigidbody2D.velocity = new Vector2(((jumped) ? pInput.GetMovement().x * this.ForwardSpeed / 2 : this._rigidbody2D.velocity.x), _rigidbody2D.velocity.y + ((jumped) ? this.JumpStrenght : 0));
        }


        //Walk
        if (this._isGrounded)
        {
            _rigidbody2D.velocity = new Vector2(pInput.GetMovement().x * this.ForwardSpeed, _rigidbody2D.velocity.y + ((jumped) ? this.JumpStrenght : 0));
        }

        //LookDirection
        if (pInput.GetMovement().x > 0)
        {
            LookDirection.SetTurnLeft();
        }
        else if (pInput.GetMovement().x < 0)
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
}
