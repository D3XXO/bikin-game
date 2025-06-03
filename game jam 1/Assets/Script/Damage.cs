using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    private playerHealth playerHealthRef;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHealthRef == null)
            {
                playerHealthRef = collision.gameObject.GetComponent<playerHealth>();
            }
        }

        if (playerHealthRef != null)
        {
            playerHealthRef.TakeDamage(damageAmount);
        }
    }
}
