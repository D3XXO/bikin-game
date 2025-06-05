using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private float delayBeforeSceneChange = 0.5f; // Optional delay to let SFX play
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.Win);
            StartCoroutine(LoadSceneAfterDelay()); // Delay scene change to ensure sound plays
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeSceneChange); // Wait for sound to play
        SceneManager.LoadScene(targetSceneName);
    }
}