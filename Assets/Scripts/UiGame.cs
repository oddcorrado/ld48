using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGame : MonoBehaviour
{
    [SerializeField] GameObject[] winPanels;
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject playPanel;
    [SerializeField] CellManager cellManager;

    public void Win(int step)
    {
        winPanels[step].SetActive(true);
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
        foreach (var go in winPanels) go.SetActive(false);
        playPanel.SetActive(true);
        cellManager.Restart();
    }
}
