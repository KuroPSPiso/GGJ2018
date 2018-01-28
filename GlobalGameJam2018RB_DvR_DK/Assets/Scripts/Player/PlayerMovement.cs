using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Parameters
    public LookDirection lookDirection;
    public float jumpStrength;
    public float forwardSpeed;
    public Transform feet;
    public GameObject trap;
    public float stunTime = 2.0f;

    //Private members
    private const int JumpCountLimit = 2;
    private Rigidbody2D _rigidbody2D = null;
    private bool _isGrounded = false;
    private bool _isWalled = false;
    private Transform _lastWallHit = null;
    private int _jumpCount = 0;
    private float stunned = 0.0f;

    PlayerInput pInput;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        pInput = GetComponent<PlayerInput>();
        _activeCollisionTrackers = new List<KeyValuePair<Collision2D, Coroutine>>();
    }

    void Update()
    {
        if (!pInput.IsActive())
            return;

        float hSpeed = this.forwardSpeed;
        float jStrength = this.jumpStrength;

        if (stunned > 0.0f)
        {
            stunned -= Time.deltaTime;
            hSpeed = 0;
            jStrength = 0;
        }

        //Jump
        bool jumped = false;

        if (this._isWalled)
        {
            if (pInput.IsJumping(true))
            {
                float jumpOffDirection = 0;
                jumped = true;
                this._jumpCount += 1;
                jumpOffDirection = (this._lastWallHit.position.x > this.transform.position.x) ? -1f : 1f;
                _rigidbody2D.velocity = new Vector2(((jumped) ? jumpOffDirection * hSpeed / 2 : this._rigidbody2D.velocity.x), _rigidbody2D.velocity.y + ((jumped) ? jStrength : 0));
            }
        }
        else if (this._isGrounded || this._jumpCount < JumpCountLimit - 1)
        {
            if (pInput.IsJumping(true))
            {
                jumped = true;
                this._jumpCount += 1;
                AudioManager.PlayerJumped();
            }
            _rigidbody2D.velocity = new Vector2(((jumped) ? pInput.GetMovement().x * hSpeed / (2 / this._jumpCount) : this._rigidbody2D.velocity.x), _rigidbody2D.velocity.y + ((jumped) ? jStrength : 0));
        }


        //Walk
        if (this._isGrounded)
        {
            _rigidbody2D.velocity = new Vector2(pInput.GetMovement().x * hSpeed, _rigidbody2D.velocity.y + ((jumped) ? jStrength : 0));
        }
        else
        {
            if (!this._isWalled)
            {
                _rigidbody2D.velocity = new Vector2(pInput.GetMovement().x * hSpeed, _rigidbody2D.velocity.y);
            }
        }

        //Trapping
        if (pInput.IsTrapping(true))
        {
            LiftOffTrap t = Instantiate(trap, transform.position, Quaternion.identity).GetComponent<LiftOffTrap>();
            t.team = pInput.team;
        }

        //LookDirection
        if (pInput.GetMovement().x > 0)
        {
            lookDirection.SetTurnLeft();
        }
        else if (pInput.GetMovement().x < 0)
        {
            lookDirection.SetTurnRight();
        }
        else
        {
            lookDirection.SetNeutral();
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8 || coll.gameObject.layer == 10)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trap")
        {
            LiftOffTrap trap = collision.GetComponent<LiftOffTrap>();

            if(trap.team != pInput.team)
            {
                if(stunned <= 0.0f)
                    stunned = stunTime;
                
                Destroy(trap.gameObject);
            }
        }
    }

    private List<KeyValuePair<Collision2D, Coroutine>> _activeCollisionTrackers;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 10)
        {
            if (feet.transform.position.y < coll.transform.position.y)
            {
                this._isWalled = false;

                bool willSkip = false;
                for (int i = 0; i < _activeCollisionTrackers.Count; i++)
                {
                    if (willSkip)
                    {
                        continue;
                    }

                    if (_activeCollisionTrackers[i].Key == coll)
                    {
                        StopCoroutine(_activeCollisionTrackers[i].Value);
                        willSkip = true;
                    }
                }

                _activeCollisionTrackers.Add(new KeyValuePair<Collision2D, Coroutine>(coll, StartCoroutine(TrackIfCanStandOnPlatform(coll))));
                Physics2D.IgnoreCollision(coll.collider, gameObject.GetComponent<Collider2D>());
            }
        }
    }

    private IEnumerator TrackIfCanStandOnPlatform(Collision2D coll)
    {
        while (feet.transform.position.y < coll.transform.position.y)
        {
            yield return null;
        }

        Physics2D.IgnoreCollision(coll.collider, gameObject.GetComponent<Collider2D>(), false);
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8 || coll.gameObject.layer == 10)
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
