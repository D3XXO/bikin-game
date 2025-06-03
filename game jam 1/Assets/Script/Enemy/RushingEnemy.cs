using System.Collections;
using UnityEngine;

public class RushingEnemy : MonoBehaviour
{
    [Header("Stealth Settings")]
    [SerializeField] private float activationRadius;
    [SerializeField] private float fadeInDuration;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    [SerializeField] private bool faceRight = true;

    [Header("Player Kill Settings")]
    [SerializeField] private int damage;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isDashing = false;
    private bool isVisible = false;
    private float lastDashTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetVisibility(false);
        FlipSprite(faceRight);
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isVisible && distanceToPlayer <= activationRadius)
            RevealEnemy();

        if (isVisible && !isDashing && Time.time >= lastDashTime + dashCooldown)
            if (distanceToPlayer <= activationRadius)
                StartCoroutine(DashForward());
    }

    private void RevealEnemy()
    {
        isVisible = true;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;
        startColor.a = 0f;
        Color endColor = startColor;
        endColor.a = 1f;

        while (elapsedTime < fadeInDuration)
        {
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = endColor;
    }

    private IEnumerator DashForward()
    {
        isDashing = true;
        lastDashTime = Time.time;

        Vector2 dashDirection = faceRight ? Vector2.right : Vector2.left;
        rb.velocity = dashDirection * dashSpeed;

        Collider2D enemyCollider = GetComponent<Collider2D>();
        if (enemyCollider != null)
            enemyCollider.isTrigger = true;

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        isDashing = false;

        if (enemyCollider != null)
            enemyCollider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDashing && other.CompareTag("Player"))
        {
            playerHealth playerHealth = other.GetComponent<playerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);
        }
    }

    private void FlipSprite(bool faceRight)
    {
        spriteRenderer.flipX = !faceRight;
    }

    private void SetVisibility(bool visible)
    {
        Color color = spriteRenderer.color;
        color.a = visible ? 1f : 0f;
        spriteRenderer.color = color;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}