using UnityEngine;

public class AutoPush : MonoBehaviour
{
    [Header("Push Settings")]
    public float pushForce = 10f;
    public float pushRadius = 0.5f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pushable"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null && !rb.isKinematic)
            {
                Vector2 pushDirection = collision.transform.position - transform.position;
                pushDirection.Normalize();

                rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, pushRadius);
    }
}