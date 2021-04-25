using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiLevelButton : MonoBehaviour
{
    [TextArea(20, 40)]
    [SerializeField] string content;

    public void PlayLevel()
    {
        CellManager.LevelContent = content;
        SceneManager.LoadScene("LevelPlayer");
    }
}
