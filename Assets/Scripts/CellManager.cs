using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CellManager : MonoBehaviour
{
    [TextArea(20,40)]
    [SerializeField] string content;
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject markerPrefab;
    [SerializeField] bool creatorMode;
    [SerializeField] UiGame uiGame;
    [SerializeField] GameObject fxDeath;
    [SerializeField] GameObject fxWater;
    [SerializeField] GameObject fxRock;
    [SerializeField] GameObject fxEarth;
    [SerializeField] GameObject topBackground;
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] AudioSource soundWin;
    [SerializeField] AudioSource soundLose;
    [SerializeField] AudioSource soundInvert;
    [SerializeField] AudioSource soundWater;
    [SerializeField] AudioSource soundDig;
    [SerializeField] AudioSource soundWind;
    [SerializeField] Transform level;
    [SerializeField] Levels levelData;

    Cell[,] cells;

    List<Cell> activeCells = new List<Cell>(); // List of currently active cells
    List<List<Cell>> history = new List<List<Cell>>(); // history for undo

    enum CellEventType { NONE, WATER, DEATH }
    class CellEvent
    {
        public Cell cell;
        public CellEventType type;
    }

    public enum MoveOutcome { DEATH, OK, OK_INVERT, WIN, WATER, NONE }
    public enum Move { UP, DOWN, LEFT, RIGHT }

    private int waterCount = 0;
    private int startX = -1;
    private bool gameOver;
    private bool isDead;

    private List<GameObject> fxs = new List<GameObject>();

    private bool playerCameraControl; 

    static public string LevelContent { get; set; }
    static public string LevelTutorial { get; set; }
    static public int LevelIndex { get; set; }

    void Start()
    {
        if(!creatorMode)
        {
            if (content == "") content = LevelContent;
            tutorialText.text = LevelTutorial;
            CreateCells();
            CheckRoots();
            CheckBackground();
        }
    }

    public void NewCells(string newContent)
    {
        content = newContent;
        Restart();
    }

    public void Restart()
    {
        gameOver = false;
        int childCount = level.childCount;

        for (int i = childCount - 1; i >= 0; i--) Destroy(level.GetChild(i).gameObject);

        waterCount = 0;
        startX = -1;
        activeCells = new List<Cell>();
        CreateCells();
        CheckRoots();
        CheckBackground();
        fxs.RemoveAll(go => { Destroy(go); return true; });
    }

    public void NextLevel()
    {
        var data = levelData.Data[Mathf.Min(levelData.Data.Length - 1, LevelIndex + 1)];
        LevelContent = data.levelData;
        LevelTutorial = data.tutorial;
        LevelIndex = Mathf.Min(levelData.Data.Length - 1, LevelIndex + 1);
        SceneManager.LoadScene("LevelPlayer");
    }

    void CreateCells()
    {
        string[] lines = content.Split('\n');
        int maxRow = lines.Length;
        int maxColumn = lines[0].Split('\t').Length;

        cells = new Cell[maxColumn, maxRow];

        for (int rowIndex = 0; rowIndex < lines.Length; rowIndex++)
        {
            int y = lines.Length - 1 - rowIndex;
            string[] objects = lines[rowIndex].Split('\t');

            if (objects.Length != maxColumn) Debug.LogError($"row {rowIndex} size is not correct {objects.Length} instead of {maxColumn}");

            for (int columnIndex = 0; columnIndex < objects.Length; columnIndex++)
            {
                int x = columnIndex;
                CreateCell(x, y, objects[columnIndex]);
            }
        }
    }

    void CheckRoots()
    {
        int x = startX;
        int y = cells.GetLength(1) - 1;
        CheckCellNeighbours(x, y, Cell.RootDirection.Up);
    }

    void CheckBackground()
    {
        topBackground.transform.position = new Vector3(10, 10 + cells.GetLength(1) - 0.5f, 0);
    }
    

    void CheckCellNeighbours(int x, int y, Cell.RootDirection previousPosition)
    {   
        if (x < 0 || x >= cells.GetLength(0) || y < 0 || y >= cells.GetLength(1)) return;

        var cell = cells[x, y];

        if (cell.InitDone) return;

        cell.InitDone = true;
        cell.PreviousPosition = previousPosition;

        List<Cell.RootDirection> directions = new List<Cell.RootDirection>();

        if(x > 0 && cells[x - 1, y].ContainsRoot)
        {
            if(previousPosition != Cell.RootDirection.Left) directions.Add(Cell.RootDirection.Left);
            CheckCellNeighbours(x - 1, y, Cell.RootDirection.Right);
        }
        if (x < cells.GetLength(0) - 1 && cells[x + 1, y].ContainsRoot)
        {
            if (previousPosition != Cell.RootDirection.Right) directions.Add(Cell.RootDirection.Right);
            CheckCellNeighbours(x + 1, y, Cell.RootDirection.Left);
        }
        if (y > 0 && cells[x, y - 1].ContainsRoot)
        {
            if (previousPosition != Cell.RootDirection.Down) directions.Add(Cell.RootDirection.Down);
            CheckCellNeighbours(x, y - 1, Cell.RootDirection.Up);
        }
        if (y < cells.GetLength(1) - 1 && cells[x, y + 1].ContainsRoot)
        {
            if (previousPosition != Cell.RootDirection.Up) directions.Add(Cell.RootDirection.Up);
            CheckCellNeighbours(x, y + 1, Cell.RootDirection.Down);
        }

        cell.Direction = directions.ToArray();
        cell.CheckEdge();
        // Debug.Log($"directions: {directions.Count}");
    }

    void CreateCell(int x, int y, string code)
    {
        var go = Instantiate(prefab);
        go.transform.position = new Vector3(x, y, 0);
        go.transform.SetParent(level);

        var goCell = go.GetComponent<Cell>();
        if (code.Contains("E")) { goCell.Back = Cell.BackType.Earth; go.name = $"Earth({x}-{y})"; }
        if (code.Contains("T"))
        {
            goCell.Back = Cell.BackType.Earth;
            go.name = $"EarthTree({x}-{y})";
            goCell.ContainsRoot = true;
        }

        if (code.Contains("A"))
        {
            goCell.Back = Cell.BackType.Earth;
            go.name = $"StartRoot({x}-{y})";
            goCell.ContainsRoot = true;
            startX = x;
        }


        if (code.Contains("S"))
        {
            goCell.Back = Cell.BackType.Earth;
            go.name = $"EarthTree({x}-{y})";
            goCell.ContainsRoot = true;

            /* var marker = Instantiate(markerPrefab);
            marker.name = $"Marker({x}-{y})";
            marker.transform.position = goCell.transform.position + new Vector3(0, 0, -1); */

            activeCells.Add(goCell);
        }

        if (code.Contains("R")) { goCell.Back = Cell.BackType.Rock; go.name = $"Rock({x}-{y})"; }
        if (code.Contains("I")) { goCell.Back = Cell.BackType.Inverter; go.name = $"Invert({x}-{y})"; }
        if (code.Contains("W")) { goCell.Back = Cell.BackType.Water; go.name = $"Water({x}-{y})"; waterCount++; }
        if (code.Contains("P")) { goCell.Back = Cell.BackType.Poison; go.name = $"Poison({x}-{y})"; }

        cells[x, y] = goCell;
    }

    MoveOutcome CheckCell(int x, int y)
    {
        if (x < 0 || x >= cells.GetLength(0) || y < 0 || y >= cells.GetLength(1)) return MoveOutcome.NONE;

        if (cells[x, y].ContainsRoot) { return MoveOutcome.NONE; }

        switch (cells[x, y].Back)
        {
            case (Cell.BackType.Rock): return MoveOutcome.NONE;
            case (Cell.BackType.Earth): return MoveOutcome.OK;
            case (Cell.BackType.Water): return MoveOutcome.WATER;
            case (Cell.BackType.Poison): return MoveOutcome.DEATH;
            case (Cell.BackType.Inverter): return MoveOutcome.OK_INVERT;
        }
        return MoveOutcome.NONE;
    }

    void ProcessRoot(Cell newCell, Cell oldCell, Cell.RootDirection previousPosition, Cell.RootDirection direction)
    {
        newCell.PreviousPosition = previousPosition;
        oldCell.Direction = new Cell.RootDirection[] { direction };
        newCell.ContainsRoot = true;
    }

    private GameObject AddFx(GameObject prefab, Vector3 position, bool destroyOnRestart = false)
    {
        var go = Instantiate(prefab);
        go.transform.position = position;
        if (destroyOnRestart) fxs.Add(go);
        return go;
    }

    public MoveOutcome ExecuteMove(Move move)
    {
        MoveOutcome outcome = MoveOutcome.OK;
        List<Cell> newCells = new List<Cell>();
        List<Cell> oldCells = new List<Cell>();

        //  Update history with activeCells
        List<Cell> historyStep = new List<Cell>(activeCells);
        history.Add(historyStep);

        // playerCameraControl = false;

        Cell.RootDirection direction = Cell.RootDirection.Up;

        switch (move)
        {
            case Move.UP: direction = Cell.RootDirection.Up; break;
            case Move.DOWN: direction = Cell.RootDirection.Down; break;
            case Move.RIGHT: direction = Cell.RootDirection.Right; break;
            case Move.LEFT: direction = Cell.RootDirection.Left; break;
        }

        activeCells.ForEach(cell =>
        {
            int newX = Mathf.RoundToInt(cell.transform.position.x); // TODO store X/Y in cell
            int newY = Mathf.RoundToInt(cell.transform.position.y); // TODO store X/Y in cell
            Cell.RootDirection previousPosition = Cell.RootDirection.Up;
            var cellDirection = direction;
            var cellMove = move;

            if (cell.IsInverted)
            {
                switch(direction)
                {
                    case Cell.RootDirection.Left: cellDirection = Cell.RootDirection.Right; break;
                    case Cell.RootDirection.Right: cellDirection = Cell.RootDirection.Left; break;
                }

                switch(move)
                {
                    case Move.LEFT: cellMove = Move.RIGHT; break;
                    case Move.RIGHT: cellMove = Move.LEFT; break;
                }
            }

            if (cell.Status != Cell.RootStatus.WATER && cell.Status != Cell.RootStatus.POISON)
            {
                switch (cellMove)
                {
                    case Move.UP: newY++; break;
                    case Move.DOWN: newY--; break;
                    case Move.RIGHT: newX++; break;
                    case Move.LEFT: newX--; break;
                }
            }



            if (newY < cell.transform.position.y) previousPosition = Cell.RootDirection.Up;
            if (newY > cell.transform.position.y) previousPosition = Cell.RootDirection.Down;
            if (newX < cell.transform.position.x) previousPosition = Cell.RootDirection.Right;
            if (newX > cell.transform.position.x) previousPosition = Cell.RootDirection.Left;

            var cellOutcome = CheckCell(newX, newY);

            switch (cellOutcome)
            {
                case MoveOutcome.NONE:
                    oldCells.Add(cell);
                    AddFx(fxRock, new Vector3(cell.transform.position.x, cell.transform.position.y, -1));
                    break;
                case MoveOutcome.DEATH:
                    ProcessRoot(cells[newX, newY], cell, previousPosition, cellDirection);
                    outcome = MoveOutcome.DEATH;
                    soundLose.Play();
                    cells[newX, newY].Fx = AddFx(fxDeath, new Vector3(newX, newY, -1), true);
                    newCells.Add(cells[newX, newY]);
                    cells[newX, newY].Status = Cell.RootStatus.POISON;
                    break;
                case MoveOutcome.OK:
                case MoveOutcome.OK_INVERT:
                    ProcessRoot(cells[newX, newY], cell, previousPosition, cellDirection);
                    AddFx(fxEarth, new Vector3(newX, newY, -2));
                    soundDig.Play();
                    if (cellOutcome == MoveOutcome.OK_INVERT) soundInvert.Play();
                    if (cellOutcome == MoveOutcome.OK_INVERT) cells[newX, newY].IsInverted = !cell.IsInverted;
                    else cells[newX, newY].IsInverted = cell.IsInverted;
                    newCells.Add(cells[newX, newY]);
                    break;
                case MoveOutcome.WATER:
                    ProcessRoot(cells[newX, newY], cell, previousPosition, cellDirection);
                    cells[newX, newY].Fx = AddFx(fxWater, new Vector3(newX, newY + 0.7f, -1), true);
                    soundWater.Play();
                    newCells.Add(cells[newX, newY]);
                    cells[newX, newY].Status = Cell.RootStatus.WATER;
                    waterCount--;
                    break;
            }
        });

        activeCells = oldCells;
        newCells.ForEach(cell => activeCells.Add(cell));

        if (outcome == MoveOutcome.DEATH)
        {
            // StartCoroutine(LoseSequence());
            isDead = true;
            return MoveOutcome.DEATH;
        }



        // ProcessRoot(newCells, direction);

        if (waterCount <= 0)
        {
            StartCoroutine(WinSequence());
            gameOver = true;
            return MoveOutcome.WIN;
        }

        return outcome;
    }

    void AdjustCamera()
    {
        if (playerCameraControl) return;

        if(activeCells.Count > 0)
        {
            var position = Camera.main.transform.position;

            float y = 0;

            activeCells.ForEach(cell => y += cell.transform.position.y);

            y = y / activeCells.Count;

            if (cells.GetLength(1) > 14) position.y = y;
            else position.y = 6;

            Camera.main.transform.position = Camera.main.transform.position * 0.99f + position * 0.01f;
        }
    }

    void CameraControl(float distance)
    {
        playerCameraControl = true;
        Camera.main.transform.position += new Vector3(0, distance, 0);
    }

    IEnumerator WinSequence()
    {
        PlayerPrefs.SetInt("BestIndex", Mathf.Max(LevelIndex, PlayerPrefs.GetInt("BestIndex", 0)));

        uiGame.Win(0);
        tutorialText.text = "";
        yield return new WaitForSeconds(0.5f);
        soundWin.Play();

        var endY = cells.GetLength(1) + 8;
        while (Camera.main.transform.position.y < endY - 1)
        {
            Camera.main.transform.position += new Vector3(0, (endY - Camera.main.transform.position.y) * 0.01f, 0);
            yield return new WaitForEndOfFrame();
        }

        uiGame.Win(1);
    }

    IEnumerator LoseSequence()
    {
        yield return new WaitForSeconds(2);
        uiGame.Lose();
    }

    void Undo()
    {
        int waterDelta = 0;
        isDead = false;

        //Repalce active cells with history cells
        if (history.Count == 0) return;
        var step = history[history.Count - 1];
        history.RemoveAt(history.Count - 1);
        activeCells.ForEach(cell =>
        {
            cell.ContainsRoot = false;
            if (cell.Status == Cell.RootStatus.WATER) waterDelta++;
            if (!step.Contains(cell) && cell.Fx != null)
            {
                Destroy(cell.Fx);
                cell.Fx = null;
            }
        });

        activeCells.RemoveAll(cell => true);
        step.ForEach(cell =>
        {
            cell.Direction = new Cell.RootDirection[] { };
            cell.ContainsRoot = true;
            activeCells.Add(cell);
            if (cell.Status == Cell.RootStatus.WATER) waterDelta--;
            if (cell.Back == Cell.BackType.Poison) isDead = true;
        });

        waterCount += waterDelta;
    }

    void Update()
    {
        var vol = Camera.main.transform.position.y / Mathf.Max(cells.GetLength(1), 2);
        soundWind.volume = vol * vol;

        if (gameOver) return;
        if (Input.GetKey(KeyCode.U)) CameraControl(1 / 16f);
        if (Input.GetKey(KeyCode.J)) CameraControl(-1 / 16f);
        if (Input.GetKey(KeyCode.R)) Restart();
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0) CameraControl(Input.mouseScrollDelta.y * 0.2f);
        if (Input.GetKeyDown(KeyCode.Backspace)) Undo();
        if (isDead) return;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) ExecuteMove(Move.UP);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) ExecuteMove(Move.DOWN);
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) ExecuteMove(Move.LEFT);
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) ExecuteMove(Move.RIGHT);


        AdjustCamera();
    }
}
