using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private int damageAmount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }

            playerHealth playerHealth = other.GetComponent<playerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}