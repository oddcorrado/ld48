using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLevelCreatorInput : MonoBehaviour
{
    [SerializeField] InputField inputData;
    [SerializeField] CellManager cellManager;
    [SerializeField] GameObject levelEditor;
    [SerializeField] Text toggleText;

    private string cellData;
    private bool hidden;

    public void EnterLevel(string data)
    {
        cellData = data;
    }

    public void Submit()
    {
        Debug.Log(cellData);
        inputData.text = "";
        cellManager.NewCells(cellData);
    }

    public void ToggleHide()
    {
        hidden = !hidden;
        levelEditor.SetActive(!hidden);
        toggleText.text = hidden ? "SHOW" : "HIDE";
    }

    public void Restart()
    {
        cellManager.Restart();
    }
}
