using UnityEngine;

public class FallingObj : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private int damageAmount;
    [SerializeField] private int requiredPasses;
    [SerializeField] private bool resetOnDeath = true;

    [Header("Falling Object Physics")]
    [SerializeField] private GameObject fallingObjectPrefab;
    [SerializeField] private float spawnHeight;
    [SerializeField] private float fallGravityScale;
    [SerializeField] private Vector2 spawnOffset = new Vector2();
    [SerializeField] private int maxSpawnedObjects;

    private int currentPasses = 0;
    private playerHealth playerHealthRef;
    private int currentlySpawnedObjects = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHealthRef == null)
            {
                playerHealthRef = other.GetComponent<playerHealth>();

                if (resetOnDeath && playerHealthRef != null)
                {
                    playerHealthRef.OnPlayerDeath += ResetObstacle;
                }
            }

            currentPasses++;

            if (currentPasses >= requiredPasses && currentlySpawnedObjects < maxSpawnedObjects)
            {
                SpawnFallingObject(other.transform.position);
            }
        }
    }

    private void SpawnFallingObject(Vector3 playerPosition)
    {
        if (fallingObjectPrefab == null) return;

        Vector3 spawnPos = new Vector3(
            playerPosition.x + spawnOffset.x,
            playerPosition.y + spawnHeight + spawnOffset.y,
            playerPosition.z
        );

        GameObject newObject = Instantiate(fallingObjectPrefab, spawnPos, Quaternion.identity);
        currentlySpawnedObjects++;

        Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = fallGravityScale;
        }

        FallingObjectDamage damage = newObject.AddComponent<FallingObjectDamage>();
        damage.Initialize(damageAmount, this);

        if (rb != null)
        {
            rb.AddForce(new Vector2(Random.Range(-2f, 2f), 0), ForceMode2D.Impulse);
        }
    }

    public void NotifyObjectDestroyed()
    {
        currentlySpawnedObjects--;
    }

    private void ResetObstacle()
    {
        currentPasses = 0;
    }

    private void OnDestroy()
    {
        if (playerHealthRef != null)
        {
            playerHealthRef.OnPlayerDeath -= ResetObstacle;
        }
    }
}

public class FallingObjectDamage : MonoBehaviour
{
    private int damageAmount;
    private FallingObj obstacleController;

    public void Initialize(int damage, FallingObj controller)
    {
        damageAmount = damage;
        obstacleController = controller;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerHealth health = collision.collider.GetComponent<playerHealth>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            Destroy(gameObject, 1f);
        }
    }

    private void OnDestroy()
    {
        if (obstacleController != null)
        {
            obstacleController.NotifyObjectDestroyed();
        }
    }
}