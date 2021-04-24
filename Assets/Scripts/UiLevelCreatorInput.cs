using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLevelCreatorInput : MonoBehaviour
{
    [SerializeField] InputField inputData;

    public void EnterLevel(string data)
    {
        Debug.Log(data);
        inputData.text = "oups";
    }
}
