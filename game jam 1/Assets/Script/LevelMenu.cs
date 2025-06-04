using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;
    public GameObject levelButtons;
   
    public void OpenLevel(int LevelId)
    {
        string levelName = "Level"+ LevelId;
        SceneManager.LoadScene(levelName);
    }
}
