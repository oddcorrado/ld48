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
    public enum BackType { Earth, Water, Poison, Rock }
    public enum RootDirection { Up, Down, Left, Right }
    /*1 = straight up
     *2 = straight side
     *3 = up right
     *4 = up left
     *5 = down right
     *6 = down left
     */

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

    private RootDirection direction;
    public RootDirection Direction
    {
        get { return direction; }
        set
        {
            direction = value; switch ((previousPosition, direction))
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
}