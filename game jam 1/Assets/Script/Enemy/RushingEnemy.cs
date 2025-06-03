using System.Collections;
using UnityEngine;

public class RushingEnemy : MonoBehaviour
{
    [Header("Stealth Settings")]
    [SerializeField] private float activationRadius = 5f;
    [SerializeField] private float fadeInDuration = 0.5f;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float dashCooldown = 2f;

    [Header("Player Kill Settings")]
    [SerializeField] private int damage = 100; // Instantly kills player if health < damage

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
        SetVisibility(false); // Start hidden

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Activate if player is close
        if (!isVisible && distanceToPlayer <= activationRadius)
            RevealEnemy();

        // Dash if visible and off cooldown
        if (isVisible && !isDashing && Time.time >= lastDashTime + dashCooldown)
            if (distanceToPlayer <= activationRadius)
                StartCoroutine(DashTowardsPlayer());
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

    private IEnumerator DashTowardsPlayer()
    {
        isDashing = true;
        lastDashTime = Time.time;

        Vector2 dashDirection = (player.position - transform.position).normalized;
        rb.velocity = dashDirection * dashSpeed;
        FlipSprite(dashDirection.x > 0);

        // Ignore walls during dash
        Collider2D enemyCollider = GetComponent<Collider2D>();
        if (enemyCollider != null)
            enemyCollider.isTrigger = true;

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        isDashing = false;

        // Re-enable collisions
        if (enemyCollider != null)
            enemyCollider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kill player on contact during dash
        if (isDashing && other.CompareTag("Player"))
        {
            playerHealth playerHealth = other.GetComponent<playerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage); // Assumes player has a health system
        }
    }

    private void FlipSprite(bool faceRight)
    {
        spriteRenderer.flipX = faceRight;
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