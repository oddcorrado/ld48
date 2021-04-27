using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UiLevelButton : MonoBehaviour
{
    [TextArea(20, 40)]
    [SerializeField] string content;
    [TextArea(2, 4)]
    [SerializeField] string tutorial;
    [SerializeField] TMP_Text displayText;
    [SerializeField] Image image;

    public string Content { get { return content; } set { content = value; } }
    public string Tutorial { get { return tutorial; } set { tutorial = value; } }
    private int index;
    public int Index
    {
        get { return index; }
        set
        {
            index = value;
            displayText.text = (value + 1).ToString();
            if (PlayerPrefs.GetInt("BestIndex", 0) < index - 1)
            {
                displayText.color = new Color(0.2f, 0.2f, 0.2f);
                image.color = Color.gray;
            }
        }
    }

    public void PlayLevel()
    {
        // if (PlayerPrefs.GetInt("BestIndex", 0) < index - 1) return;
        CellManager.LevelContent = content;
        CellManager.LevelTutorial = tutorial;
        CellManager.LevelIndex = Index;
        SceneManager.LoadScene("LevelPlayer");
    }
}
