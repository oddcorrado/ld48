using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite[] backgroundSprites;
    [SerializeField] private SpriteRenderer backgroundSprite;
    public enum BackType { Earth, Water, Poison, Rock }
    public enum RootDirection { Up, Down, Left, Right }


    private BackType back;
    public BackType Back
    {
        get { return back; }
        set { back = value; backgroundSprite.sprite = backgroundSprites[(int)value]; }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
