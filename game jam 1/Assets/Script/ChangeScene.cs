using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private float delayBeforeSceneChange;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Canvas[] allCanvas;
    [SerializeField] private float fadeDuration;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (Canvas canvas in allCanvas)
            {
                canvas.enabled = false;
            }

            var playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null) playerMovement.enabled = false;

            var health = collision.gameObject.GetComponent<playerHealth>();
            if (health != null) health.SetWinningState(true);

            audioManager.PlaySFX(audioManager.Win);
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        float elapsedTime = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(1, 1, 1, 1);

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = targetColor;

        yield return new WaitForSeconds(delayBeforeSceneChange);
        SceneManager.LoadScene(targetSceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (allCanvas != null)
        {
            foreach (Canvas canvas in allCanvas)
            {
                canvas.enabled = true;
            }
        }
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}