using System.Collections;
using UnityEngine;

public class DisappearBlock : MonoBehaviour
{
    [Header("Disappear Settings")]
    [SerializeField] public float disappearDelay = 0.5f; // Time before block disappears
    [SerializeField] public float respawnTime = 3f; // Time before block reappears
    [SerializeField] public bool permanentDisappear = false; // If true, won't respawn

    [Header("Visual Feedback")]
    [SerializeField] private ParticleSystem disappearParticles;
    [SerializeField] private AudioClip disappearSound;

    private Collider2D blockCollider;
    private SpriteRenderer blockRenderer;
    private bool isVisible = true;

    private void Start()
    {
        blockCollider = GetComponent<Collider2D>();
        blockRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player landed on top of the block
        if (collision.gameObject.CompareTag("Player") && IsPlayerAbove(collision))
        {
            StartCoroutine(DisappearSequence());
        }
    }

    private bool IsPlayerAbove(Collision2D collision)
    {
        // Check if player is coming from above
        return collision.relativeVelocity.y <= 0;
    }

    private IEnumerator DisappearSequence()
    {
        // Visual/Sound feedback
        if (disappearParticles != null)
            disappearParticles.Play();

        if (disappearSound != null)
            AudioSource.PlayClipAtPoint(disappearSound, transform.position);

        // Wait before disappearing
        yield return new WaitForSeconds(disappearDelay);

        // Make block invisible and non-collidable
        blockCollider.enabled = false;
        blockRenderer.enabled = false;
        isVisible = false;

        // Respawn if not permanent
        if (!permanentDisappear)
        {
            yield return new WaitForSeconds(respawnTime);
            RespawnBlock();
        }
    }

    private void RespawnBlock()
    {
        blockCollider.enabled = true;
        blockRenderer.enabled = true;
        isVisible = true;
    }

    // For debugging
    private void OnDrawGizmos()
    {
        if (!isVisible)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>().bounds.size);
        }
    }
}