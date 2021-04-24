using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    [TextArea(20,40)]
    [SerializeField] string content;
    [SerializeField] GameObject prefab;

    Cell[,] cells;

    List<Cell> activeCells = new List<Cell>();

    public enum MoveOutcome { DEATH, OK, WIN, WATER, NONE }
    public enum Move { UP, DOWN, LEFT, RIGHT }

    private int waterCount = 0;

    void Start()
    {
        CreateCells();
    }

    void CreateCells()
    {
        string[] lines = content.Split('\n');
        int maxRow = lines.Length;
        int maxColumn = lines[0].Split(',').Length;

        cells = new Cell[maxColumn, maxRow];

        for (int rowIndex = 0; rowIndex < lines.Length; rowIndex++)
        {
            int y = lines.Length - 1 - rowIndex;
            string[] objects = lines[rowIndex].Split(',');

            if (objects.Length != maxColumn) Debug.LogError($"row {rowIndex} size is not correct {objects.Length} instead of {maxColumn}");

            for (int columnIndex = 0; columnIndex < objects.Length; columnIndex++)
            {
                int x = columnIndex;
                CreateCell(x, y, objects[columnIndex]);
            }
        }
    }

    void CreateCell(int x, int y, string code)
    {
        var go = Instantiate(prefab);
        go.transform.position = new Vector3(x, y, 0);
        go.transform.SetParent(gameObject.transform);

        var goCell = go.GetComponent<Cell>();
        if (code.Contains("E")) { goCell.Back = Cell.BackType.Earth; go.name = $"Earth({x}-{y})"; }
        if (code.Contains("T")) { goCell.Back = Cell.BackType.Earth; go.name = $"EarthTree({x}-{y})"; }
        if (code.Contains("R")) { goCell.Back = Cell.BackType.Rock; go.name = $"Rock({x}-{y})"; }
        if (code.Contains("W")) { goCell.Back = Cell.BackType.Water; go.name = $"Water({x}-{y})"; waterCount++; }
        if (code.Contains("P")) { goCell.Back = Cell.BackType.Poison; go.name = $"Poison({x}-{y})"; }

        cells[x, y] = goCell;
    }

    MoveOutcome CheckCell(int x, int y)
    {
        if (x < 0 || x >= cells.GetLength(0) || y < 0 || y >= cells.GetLength(1)) return MoveOutcome.NONE;

        switch(cells[x, y].Back)
        {
            case (Cell.BackType.Rock): return MoveOutcome.NONE;
            case (Cell.BackType.Earth): return MoveOutcome.OK;
            case (Cell.BackType.Water): return MoveOutcome.WATER;
            case (Cell.BackType.Poison): return MoveOutcome.DEATH;
        }
        return MoveOutcome.NONE;
    }

    public MoveOutcome ExecuteMove(Move move)
    {
        MoveOutcome outcome = MoveOutcome.OK;
        List<Cell> newCells = new List<Cell>();

        activeCells.ForEach(cell =>
        {
            int newX = Mathf.RoundToInt(cell.transform.position.x); // TODO store X/Y in cell
            int newY = Mathf.RoundToInt(cell.transform.position.y); // TODO store X/Y in cell 
            switch (move)
            {
                case Move.UP: newY++; break;
                case Move.DOWN: newY--; break;
                case Move.RIGHT: newX++; break;
                case Move.LEFT: newX--; break;
            }

            var cellOutcome = CheckCell(newX, newY);

            switch (cellOutcome)
            {
                case MoveOutcome.NONE: newCells.Add(cell); break;
                case MoveOutcome.DEATH: outcome = MoveOutcome.DEATH; break;
                case MoveOutcome.OK: newCells.Add(cells[newX, newY]); break;
                case MoveOutcome.WATER: waterCount--; break;
            }
        });

        if (outcome == MoveOutcome.DEATH) return MoveOutcome.DEATH;

        activeCells = newCells;

        if (waterCount <= 0) outcome = MoveOutcome.WIN;
        
        return outcome;
    }
}
