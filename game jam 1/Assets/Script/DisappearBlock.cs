using System.Collections;
using UnityEngine;

public class DisappearBlock : MonoBehaviour
{
    [Header("Disappear Settings")]
    [SerializeField] public float disappearDelay;
    [SerializeField] public float respawnTime;
    [SerializeField] public bool permanentDisappear = false;

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
        if (collision.gameObject.CompareTag("Player") && IsPlayerAbove(collision))
        {
            StartCoroutine(DisappearSequence());
        }
    }

    private bool IsPlayerAbove(Collision2D collision)
    {
        return collision.relativeVelocity.y <= 0;
    }

    private IEnumerator DisappearSequence()
    {
        yield return new WaitForSeconds(disappearDelay);

        blockCollider.enabled = false;
        blockRenderer.enabled = false;
        isVisible = false;

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

    private void OnDrawGizmos()
    {
        if (!isVisible)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>().bounds.size);
        }
    }
}