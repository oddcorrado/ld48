using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGame : MonoBehaviour
{
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject playPanel;
    [SerializeField] CellManager cellManager;

    public void Win()
    {
        winPanel.SetActive(true);
        playPanel.SetActive(false);
    }

    public void Lose()
    {
        losePanel.SetActive(true);
        playPanel.SetActive(false);
    }

    public void Restart()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        playPanel.SetActive(true);
        cellManager.Restart();
    }
}
