using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Cell[,] cells;

    List<Cell> activeCells = new List<Cell>();

    public enum MoveOutcome { DEATH, OK, WIN, WATER, NONE }
    public enum Move { UP, DOWN, LEFT, RIGHT }

    private int waterCount = 0;
    private int startX = -1;
    private bool gameOver;

    private List<GameObject> fxs = new List<GameObject>();

    static public string LevelContent { get; set; }

    void Start()
    {
        if(!creatorMode)
        {
            if (content == "") content = LevelContent;
            CreateCells();
            CheckRoots();
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
        int childCount = transform.childCount;

        for (int i = childCount - 1; i >= 0; i--) Destroy(transform.GetChild(i).gameObject);

        waterCount = 0;
        startX = -1;
        activeCells = new List<Cell>();
        CreateCells();
        CheckRoots();
        fxs.RemoveAll(go => { Destroy(go); return true; });
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
        Debug.Log($"directions: {directions.Count}");
    }

    void CreateCell(int x, int y, string code)
    {
        var go = Instantiate(prefab);
        go.transform.position = new Vector3(x, y, 0);
        go.transform.SetParent(gameObject.transform);

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
        }
        return MoveOutcome.NONE;
    }

    void ProcessRoot(Cell newCell, Cell oldCell, Cell.RootDirection previousPosition, Cell.RootDirection direction)
    {
        Debug.Log($"{oldCell.name} {previousPosition} {direction}");

        newCell.PreviousPosition = previousPosition;
        oldCell.Direction = new Cell.RootDirection[] { direction };
        newCell.ContainsRoot = true;
    }

    private void AddFx(GameObject prefab, Vector3 position, bool destroyOnRestart = false)
    {
        var go = Instantiate(prefab);
        go.transform.position = position;
        if (destroyOnRestart) fxs.Add(go);
    }

    public MoveOutcome ExecuteMove(Move move)
    {
        MoveOutcome outcome = MoveOutcome.OK;
        List<Cell> newCells = new List<Cell>();
        List<Cell> oldCells = new List<Cell>();

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

            switch (move)
            {
                case Move.UP: newY++; break;
                case Move.DOWN: newY--; break;
                case Move.RIGHT: newX++; break;
                case Move.LEFT: newX--; break;
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
                    ProcessRoot(cells[newX, newY], cell, previousPosition, direction);
                    outcome = MoveOutcome.DEATH;
                    AddFx(fxDeath, new Vector3(newX, newY, -1), true);
                    break;
                case MoveOutcome.OK:
                    ProcessRoot(cells[newX, newY], cell, previousPosition, direction);
                    AddFx(fxEarth, new Vector3(newX, newY, -1));
                    newCells.Add(cells[newX, newY]);
                    break;
                case MoveOutcome.WATER:
                    ProcessRoot(cells[newX, newY], cell, previousPosition, direction);
                    AddFx(fxWater, new Vector3(newX, newY, -1), true);
                    /* newCells.Add(cells[newX, newY]); */
                    waterCount--;
                    break;
            }
        });

        if (outcome == MoveOutcome.DEATH)
        {
            uiGame.Lose();
            gameOver = true;
            return MoveOutcome.DEATH;
        }

        activeCells = oldCells;
        newCells.ForEach(cell => activeCells.Add(cell));

        // ProcessRoot(newCells, direction);

        if (waterCount <= 0)
        {
            uiGame.Win();
            gameOver = true;
            return MoveOutcome.WIN;
        }

        return outcome;
    }

    void AdjustCamera()
    {
        if(cells.GetLength(1) > 20 && activeCells.Count > 0)
        {
            var position = Camera.main.transform.position;

            float y = 0;

            activeCells.ForEach(cell => y += cell.transform.position.y);

            y = y / activeCells.Count;

            position.y = y;

            Camera.main.transform.position = Camera.main.transform.position * 0.99f + position * 0.01f;
        }
    }

    void Update()
    {
        if (gameOver) return;
        if (Input.GetKeyDown(KeyCode.UpArrow)) ExecuteMove(Move.UP);
        if (Input.GetKeyDown(KeyCode.DownArrow)) ExecuteMove(Move.DOWN);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ExecuteMove(Move.LEFT);
        if (Input.GetKeyDown(KeyCode.RightArrow)) ExecuteMove(Move.RIGHT);

        AdjustCamera();
    }
}
