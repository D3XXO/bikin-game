using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private int requiredPasses = 1;
    [SerializeField] private bool resetOnDeath = true;

    [Header("Falling Object Physics")]
    [SerializeField] private GameObject fallingObjectPrefab;
    [SerializeField] private float spawnHeight = 10f;
    [SerializeField] private float fallGravityScale = 2f;
    [SerializeField] private Vector2 spawnOffset = new Vector2(0, 2f);
    [SerializeField] private int maxSpawnedObjects = 5;

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

        // Calculate spawn position above player
        Vector3 spawnPos = new Vector3(
            playerPosition.x + spawnOffset.x,
            playerPosition.y + spawnHeight + spawnOffset.y,
            playerPosition.z
        );

        GameObject newObject = Instantiate(fallingObjectPrefab, spawnPos, Quaternion.identity);
        currentlySpawnedObjects++;

        // Setup physics
        Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = fallGravityScale;
        }

        // Add damage component
        FallingObjectDamage damage = newObject.AddComponent<FallingObjectDamage>();
        damage.Initialize(damageAmount, this);

        // Optional: Add random horizontal force for variety
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
        // Note: Existing falling objects will persist unless you add cleanup logic
    }

    private void OnDestroy()
    {
        if (playerHealthRef != null)
        {
            playerHealthRef.OnPlayerDeath -= ResetObstacle;
        }
    }
}

// Helper component for falling objects
public class FallingObjectDamage : MonoBehaviour
{
    private int damageAmount;
    private Obstacle obstacleController;

    public void Initialize(int damage, Obstacle controller)
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
            // Destroy when hitting ground after a delay
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