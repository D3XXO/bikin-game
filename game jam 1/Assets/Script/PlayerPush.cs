using UnityEngine;

public class AutoPush : MonoBehaviour
{
    [Header("Push Settings")]
    public float pushForce = 10f;
    public float pushRadius = 0.5f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Only push objects with the "Pushable" tag
        if (collision.gameObject.CompareTag("Pushable"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null && !rb.isKinematic)
            {
                // Calculate push direction (player -> object)
                Vector2 pushDirection = collision.transform.position - transform.position;
                pushDirection.Normalize();

                rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }

    // Visualize push radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, pushRadius);
    }
}