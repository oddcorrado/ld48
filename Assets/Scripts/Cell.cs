using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite[] backgroundSprites;
    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private SpriteRenderer rootSprite;
    [SerializeField] private Sprite[] rootStraights;
    [SerializeField] private Sprite[] rootAngles;
    [SerializeField] private Sprite rootEdge;
    [SerializeField] private Sprite rootCross;
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

    public enum BackType { Earth, Water, Poison, Rock, Inverter }
    public enum RootDirection { Up, Down, Left, Right }
    public enum RootStatus { OK, POISON, WATER}
    /*1 = straight up
     *2 = straight side
     *3 = up right
     *4 = up left
     *5 = down right
     *6 = down left
     */

    public bool InitDone;
    public bool IsInverted;
    public RootStatus Status;
    public GameObject Fx;

    private bool containsRoot = false;
    public bool ContainsRoot
    {
        get { return containsRoot; }
        set
        {
            containsRoot = value;
            int rotation = 0;
            if(containsRoot == true)
            {
                switch (PreviousPosition)
                {
                    case RootDirection.Up: rotation = 0; rootSprite.sprite = rootEdge; break;
                    case RootDirection.Down: rotation = 180; rootSprite.sprite = rootEdge; break;
                    case RootDirection.Left: rotation = 90; rootSprite.sprite = rootEdge; break;
                    case RootDirection.Right: rotation = 270; rootSprite.sprite = rootEdge; break;
                }
            }
            else
            {
                rootSprite.sprite = null;
            }
            rootSprite.transform.localRotation = Quaternion.Euler(0, 0, rotation);
        }
    }
    private BackType back;
    public BackType Back
    {
        get { return back; }
        set { back = value; backgrounds[(int)value].SetActive(true); }
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

    public void CheckEdge()
    {
        if(Direction.Length < 1)
        {
            int rotation = 0;
            switch (PreviousPosition)
            {
                case RootDirection.Up: rotation = 0; rootSprite.sprite = rootEdge; break;
                case RootDirection.Down: rotation = 180; rootSprite.sprite = rootEdge; break;
                case RootDirection.Left: rotation = 90; rootSprite.sprite = rootEdge; break;
                case RootDirection.Right: rotation = 270; rootSprite.sprite = rootEdge; break;
            }
            rootSprite.transform.localRotation = Quaternion.Euler(0, 0, rotation);
        }
    }


    void ProcessDirectionSingle()
    {
        switch ((previousPosition, directions[0]))
        {

            case (RootDirection.Up, RootDirection.Down):
                rootSprite.sprite = rootStraights[Random.Range(0, rootStraights.Length)];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 270);
                break;
            case (RootDirection.Up, RootDirection.Right):
                rootSprite.sprite = rootAngles[0];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);

                break;
            case (RootDirection.Up, RootDirection.Left):
                rootSprite.sprite = rootAngles[3];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);

                break;
            case (RootDirection.Down, RootDirection.Up):
                rootSprite.sprite = rootStraights[Random.Range(0, rootStraights.Length)];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 270);

                break;
            case (RootDirection.Down, RootDirection.Right):
                rootSprite.sprite = rootAngles[2];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case (RootDirection.Down, RootDirection.Left):
                rootSprite.sprite = rootAngles[1];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);

                break;
            case (RootDirection.Right, RootDirection.Up):
                rootSprite.sprite = rootAngles[0];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);

                break;
            case (RootDirection.Right, RootDirection.Down):
                rootSprite.sprite = rootAngles[2];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case (RootDirection.Right, RootDirection.Left):
                rootSprite.sprite = rootStraights[Random.Range(0, rootStraights.Length)];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case (RootDirection.Left, RootDirection.Up):
                rootSprite.sprite = rootAngles[3];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);

                break;
            case (RootDirection.Left, RootDirection.Down):
                rootSprite.sprite = rootAngles[1];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);

                break;
            case (RootDirection.Left, RootDirection.Right):
                rootSprite.sprite = rootStraights[0];
                rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);

                break;


            default:

                break;
        };
    }

    void ProcessDirectionDouble()
    {
        switch ((previousPosition, directions[0], directions[1]))
        {

            case (RootDirection.Up, RootDirection.Down, RootDirection.Right):
                rootSprite.sprite = UpDownRight;
                break;
            case (RootDirection.Up, RootDirection.Right, RootDirection.Down):
                rootSprite.sprite = UpDownRight;
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
        rootSprite.sprite = rootCross;
        rootSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}