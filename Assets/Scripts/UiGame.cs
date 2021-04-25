using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGame : MonoBehaviour
{
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] CellManager cellManager;

    public void Win()
    {
        winPanel.SetActive(true);
    }

    public void Lose()
    {
        losePanel.SetActive(true);
    }

    public void Restart()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        cellManager.Restart();
    }
}
