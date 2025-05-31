using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 20f;
    [SerializeField] private float teleportDistance = 3f;
    [SerializeField] private LayerMask obstacleLayer;

    private float horizontalInput;
    private bool isFacingRight = false;
    private bool isGrounded = false;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        FlipSprite();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetMouseButtonDown(2))
        {
            Teleport();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    void Teleport()
    {
        float direction = isFacingRight ? 1f : -1f;
        Vector2 teleportDirection = new Vector2(direction, 0f);

        float actualDistance = CalculateSafeTeleportDistance(teleportDirection);

        if (actualDistance > 0.1f)
        {
            Vector2 newPosition = new Vector2(
                transform.position.x + (actualDistance * direction),
                transform.position.y
            );
            transform.position = newPosition;
        }
    }

    float CalculateSafeTeleportDistance(Vector2 direction)
    {
        float playerWidth = GetComponent<Collider2D>().bounds.size.x / 2;
        RaycastHit2D hit = Physics2D.BoxCast(
            origin: (Vector2)transform.position + (direction * playerWidth),
            size: new Vector2(0.1f, GetComponent<Collider2D>().bounds.size.y * 0.9f),
            angle: 0f,
            direction: direction,
            distance: teleportDistance,
            layerMask: obstacleLayer
        );

        if (hit.collider != null)
        {
            return Mathf.Max(0, hit.distance - 0.1f);
        }

        return teleportDistance;
    }

    void FlipSprite()
    {
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }
}
