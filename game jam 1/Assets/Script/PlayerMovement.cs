using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
<<<<<<< Updated upstream
    public float moveSpeed = 5f;
    bool isFacingRight = false;
    public float jumpPower = 5f;
    bool isJumping = false;
=======
    float moveSpeed = 5f;
    bool isFacingRight = false;
    public float jumpPower = 20f;
    bool isGrounded = false;
>>>>>>> Stashed changes

    Rigidbody2D rb;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
<<<<<<< Updated upstream
            isJumping = true;
=======
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
>>>>>>> Stashed changes
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
<<<<<<< Updated upstream
=======
        animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
>>>>>>> Stashed changes
    }

    void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Ensure your ground objects have the "Ground" tag
        {
            isGrounded = true;
            animator.SetBool("isJumping", !isGrounded);
        }
    }
}
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
