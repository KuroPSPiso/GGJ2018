using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Parameters
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;

    //Private members
    bool grounded = false;
    bool jump = false;
    Rigidbody2D rb;

    PlayerInput pInput;

    void Start()
    {
        pInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (grounded && pInput.IsJumping())
            jump = true;
    }

    void FixedUpdate()
    {
        float h = pInput.GetMovement().x;

        if (h * rb.velocity.x < maxSpeed)
            rb.AddForce(Vector2.right * h * moveForce);

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

        if (jump)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }
}
