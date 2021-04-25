using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiLevelButton : MonoBehaviour
{
    [TextArea(20, 40)]
    [SerializeField] string content;
    [TextArea(2, 4)]
    [SerializeField] string tutorial;

    public void PlayLevel()
    {
        CellManager.LevelContent = content;
        CellManager.LevelTutorial = tutorial;
        SceneManager.LoadScene("LevelPlayer");
    }
}
