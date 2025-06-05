using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private float dieAnimationLength;
    private bool isDead = false;

    public event Action OnPlayerDeath;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        maxHealth = health;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if(health <= 0 && !isDead)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isDead)
        {
            health = 0;
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            health -= damage;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        audioManager.PlaySFX(audioManager.Die);

        OnPlayerDeath?.Invoke();

        var movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;

        var enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            enemy.enabled = false;
        }

        var elevators = FindObjectsOfType<Elevator>();
        foreach (var elevator in elevators)
        {
            elevator.enabled = false;
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Die");
            StartCoroutine(ShowGameOverAfterAnimation());
        }
        else
        {
            ShowGameOver();
        }
    }

    private IEnumerator ShowGameOverAfterAnimation()
    {
        yield return new WaitForSeconds(dieAnimationLength);
        ShowGameOver();
    }

    private void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}