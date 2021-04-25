using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    static bool IsRunning { get; set; }
    void Start()
    {
        if (IsRunning) Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            IsRunning = true;
        }
        
    }
}
