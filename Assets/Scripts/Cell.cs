using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite[] backgroundSprites;
    [SerializeField] private SpriteRenderer backgroundSprite;
    [SerializeField] private SpriteRenderer rootSprite;
    [SerializeField] private Sprite[] rootStraights;
    [SerializeField] private Sprite[] rootAngles;
    [SerializeField] private Sprite rootEdgePreviousLeft;
    [SerializeField] private Sprite rootEdgePreviousRight;
    [SerializeField] private Sprite rootEdgePreviousUp;
    [SerializeField] private Sprite rootEdgePreviousDown;
    [SerializeField] private Sprite rootEdge3Directions;
    [SerializeField] private Sprite UpDownRight;
    [SerializeField] private Sprite UpDownLeft;
    [SerializeField] private Sprite UpRightLeft;
    [SerializeField] private Sprite DownUpRight;
    [SerializeField] private Sprite DownUpLeft;
    [SerializeField] private Sprite DownRightLeft;
    [SerializeField] private Sprite RightDownUp;
    [SerializeField] private Sprite RightDownLeft;
    [SerializeField] private Sprite RightUpLeft;
    [SerializeField] private Sprite LeftDownUp;
    [SerializeField] private Sprite LeftDownRight;
    [SerializeField] private Sprite LeftUpRight;

    public enum BackType { Earth, Water, Poison, Rock }
    public enum RootDirection { Up, Down, Left, Right }
   

    public bool InitDone;

    private bool containsRoot = false;
    public bool ContainsRoot
    {
        get { return containsRoot; }
        set
        {
            containsRoot = value;
            switch (PreviousPosition)
            {
                case RootDirection.Up: rootSprite.sprite = rootEdgePreviousUp; break;
                case RootDirection.Down: rootSprite.sprite = rootEdgePreviousDown; break;
                case RootDirection.Left: rootSprite.sprite = rootEdgePreviousLeft; break;
                case RootDirection.Right: rootSprite.sprite = rootEdgePreviousRight; break;
            }
        }
    }
    private BackType back;
    public BackType Back
    {
        get { return back; }
        set { back = value; backgroundSprite.sprite = backgroundSprites[(int)value]; }
    }

    private RootDirection[] directions;
    public RootDirection[] Direction
    {
        get { return directions; }
        set
        {
            directions = value;
            switch (directions.Length)
            {
                case 1: ProcessDirectionSingle(); break;
                case 2: ProcessDirectionDouble(); break;
                case 3: ProcessDirectionTriple(); break;
            }
        }
    }

    private RootDirection previousPosition;
    public RootDirection PreviousPosition
    {
        get { return previousPosition; }
        set { previousPosition = value; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ProcessDirectionSingle()
    {
        switch ((previousPosition, directions[0]))
        {

            case (RootDirection.Up, RootDirection.Down):
                rootSprite.sprite = rootStraights[Random.Range(0, rootStraights.Length)];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
            case (RootDirection.Up, RootDirection.Right):
                rootSprite.sprite = rootAngles[Random.Range(0, rootAngles.Length)];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 90);

                break;
            case (RootDirection.Up, RootDirection.Left):
                rootSprite.sprite = rootAngles[Random.Range(0, rootAngles.Length)];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 180, 90);

                break;
            case (RootDirection.Down, RootDirection.Up):
                rootSprite.sprite = rootStraights[Random.Range(0, rootStraights.Length)];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 90);

                break;
            case (RootDirection.Down, RootDirection.Right):
                rootSprite.sprite = rootAngles[Random.Range(0, rootAngles.Length)];

                break;
            case (RootDirection.Down, RootDirection.Left):
                rootSprite.sprite = rootAngles[Random.Range(0, rootAngles.Length)];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 180, 0);

                break;
            case (RootDirection.Right, RootDirection.Up):
                rootSprite.sprite = rootAngles[Random.Range(0, rootAngles.Length)];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 90);

                break;
            case (RootDirection.Right, RootDirection.Down):
                rootSprite.sprite = rootAngles[Random.Range(0, rootAngles.Length)];

                break;
            case (RootDirection.Right, RootDirection.Left):
                rootSprite.sprite = rootStraights[Random.Range(0, rootStraights.Length)];

                break;
            case (RootDirection.Left, RootDirection.Up):
                rootSprite.sprite = rootAngles[Random.Range(0, rootAngles.Length)];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 180, 90);

                break;
            case (RootDirection.Left, RootDirection.Down):
                rootSprite.sprite = rootAngles[1];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 180, 0);

                break;
            case (RootDirection.Left, RootDirection.Right):
                rootSprite.sprite = rootStraights[0];

                break;


            default:

                break;
        };
    }

    void ProcessDirectionDouble()
    {
        switch ((previousPosition, directions[0], directions[1]))
        {

            case (RootDirection.Up, RootDirection.Down, RootDirection.Right ):
                rootSprite.sprite = UpDownRight;
                rootSprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
                break;
            case (RootDirection.Up, RootDirection.Right, RootDirection.Down):
                rootSprite.sprite = UpDownRight;
                rootSprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
                break;
            case (RootDirection.Up, RootDirection.Down, RootDirection.Left):
                rootSprite.sprite = UpDownLeft;
                break;
            case (RootDirection.Up, RootDirection.Left, RootDirection.Down):
                rootSprite.sprite = UpDownLeft;
                break;
            case (RootDirection.Up, RootDirection.Left, RootDirection.Right):
                rootSprite.sprite = UpRightLeft;
                break;
            case (RootDirection.Up, RootDirection.Right, RootDirection.Left):
                rootSprite.sprite = UpRightLeft;
                break;



            case (RootDirection.Down, RootDirection.Up, RootDirection.Right):
                rootSprite.sprite = DownUpRight;
                break;
            case (RootDirection.Down, RootDirection.Right, RootDirection.Up):
                rootSprite.sprite = DownUpRight;
                break;
            case (RootDirection.Down, RootDirection.Up, RootDirection.Left):
                rootSprite.sprite = DownUpLeft;
                break;
            case (RootDirection.Down, RootDirection.Left, RootDirection.Up):
                rootSprite.sprite = DownUpLeft;
                break;
            case (RootDirection.Down, RootDirection.Left, RootDirection.Right):
                rootSprite.sprite = DownRightLeft;
                break;
            case (RootDirection.Down, RootDirection.Right, RootDirection.Left):
                rootSprite.sprite = DownRightLeft;
                break;




            case (RootDirection.Right, RootDirection.Down, RootDirection.Up):
                rootSprite.sprite = RightDownUp;
                break;
            case (RootDirection.Right, RootDirection.Up, RootDirection.Down):
                rootSprite.sprite = RightDownUp;
                break;
            case (RootDirection.Right, RootDirection.Down, RootDirection.Left):
                rootSprite.sprite = RightDownLeft;
                break;
            case (RootDirection.Right, RootDirection.Left, RootDirection.Down):
                rootSprite.sprite = RightDownLeft;
                break;
            case (RootDirection.Right, RootDirection.Up, RootDirection.Left):
                rootSprite.sprite = RightUpLeft;
                break;
            case (RootDirection.Right, RootDirection.Left, RootDirection.Up):
                rootSprite.sprite = RightUpLeft;
                break;




            case (RootDirection.Left, RootDirection.Down, RootDirection.Up):
                rootSprite.sprite = LeftDownUp;
                break;
            case (RootDirection.Left, RootDirection.Up, RootDirection.Down):
                rootSprite.sprite = LeftDownUp;
                break;
            case (RootDirection.Left, RootDirection.Down, RootDirection.Right):
                rootSprite.sprite = LeftDownRight;
                break;
            case (RootDirection.Left, RootDirection.Right, RootDirection.Down):
                rootSprite.sprite = LeftDownRight;
                break;
            case (RootDirection.Left, RootDirection.Up, RootDirection.Right):
                rootSprite.sprite = LeftUpRight;
                break;
            case (RootDirection.Left, RootDirection.Right, RootDirection.Up):
                rootSprite.sprite = LeftUpRight;
                break;

            default:

                break;
        };
    }

    void ProcessDirectionTriple()
    {
        rootSprite.sprite = rootEdge3Directions;
    }
}