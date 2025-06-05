using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject[] objectToActivate;
    [SerializeField] private int damageAmount;
    [SerializeField] private int requiredPasses;
    private bool resetOnDeath = true;
    
    private int currentPasses = 0;
    private playerHealth playerHealthRef;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.Spike);
            if (playerHealthRef == null)
            {
                playerHealthRef = other.GetComponent<playerHealth>();
                
                if (resetOnDeath && playerHealthRef != null)
                {
                    playerHealthRef.OnPlayerDeath += ResetPassCount;
                }
            }

            currentPasses++;
            
            if (currentPasses == requiredPasses)
            {
                ActivateObstacle(other);
            }
        }
    }

    private void ActivateObstacle(Collider2D player)
    {
        if (objectToActivate != null && objectToActivate.Length > 0)
        {
            foreach (GameObject obj in objectToActivate)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }

        if (playerHealthRef != null)
        {
            playerHealthRef.TakeDamage(damageAmount);
        }
    }

    private void ResetPassCount()
    {
        currentPasses = 0;
    }

    private void OnDestroy()
    {
        if (playerHealthRef != null)
        {
            playerHealthRef.OnPlayerDeath -= ResetPassCount;
        }
    }
}