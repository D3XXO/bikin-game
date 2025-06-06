using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void OpenLevel(int LevelId)
    {
        string levelName = "Level" + LevelId;
        SceneManager.LoadScene(levelName);
    }

    public void CloseTab()
    {
        gameObject.SetActive(false);
    }
}
