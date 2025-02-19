using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MineField : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] MinefieldVisualizer visualizer;
    [SerializeField] GameResultManager gameResultManager;
    [Range(15, 50)][SerializeField] int bombPercentage;

    List<Cell> cells = new List<Cell>();
    Dictionary<Vector3Int, Cell> positionToCell = new Dictionary<Vector3Int, Cell>();

    int totalCells;
    int bombsToSetup;
    int remainedBombs;
    int settedFlags = 0;
    int closedCells;

    public int Width { get => width; }
    public int Height { get => height; }

    private void Awake()
    {
        width = GameData.FieldWidth;
        height = GameData.FieldHeight;

        totalCells = width * height;
        bombsToSetup = totalCells * bombPercentage / 100;
        remainedBombs = bombsToSetup;
        closedCells = totalCells;
    }

    private void Start()
    {
        InputManager inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            inputManager.SetMineField(this);
            inputManager.EnableInput(true); 
        }

        StartGame();
    }

    public void StartGame()
    {
        CreateMinefield();
        visualizer.VisualizeCellsOnStart(cells);
        Camera.main.orthographicSize = height / 2f + 2.5f;
        Camera.main.transform.position = new Vector3(width / 2f - 0.5f, height / 2f - 0.5f, -10);
        OpenRandomEmptyCell();
    }

    private void CreateMinefield()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cell cell = new Cell(i, j);
                cells.Add(cell);
                positionToCell[new Vector3Int(i, j, 0)] = cell;
            }
        }
        SetBombs();
    }

    private void SetBombs()
    {
        int setBombs = 0;
        while (setBombs < bombsToSetup)
        {
            int randomIndex = Random.Range(0, cells.Count);
            if (cells[randomIndex].IsBomb) continue;
            cells[randomIndex].IsBomb = true;
            setBombs++;
        }
    }

    public void OpenCellByCoords(Vector3Int cellCoords)
    {
        Cell cell = positionToCell[cellCoords];
        OpenCellResult result = cell.OpenCell();
        if (result == OpenCellResult.Opened)
        {
            int bombsAround = GetBombsAroundCell(cell);
            visualizer.OpenCell(cell, bombsAround);
            closedCells--;
            if (bombsAround == 0)
            {
                foreach (Cell neighbour in GetNeighbourCells(cell))
                {
                    OpenCellByCoords(new Vector3Int(neighbour.XCoord, neighbour.YCoord, 0));
                }
            }
        }
        if (result == OpenCellResult.Gameover)
        {
            ShowGameResult("Вы проиграли");
        }
        if (closedCells == bombsToSetup)
        {
            ShowGameResult("Вы выиграли");
        }
    }

    private void ShowGameResult(string message)
    {
        if (gameResultManager != null)
        {
            int difficulty = GetDifficulty(); // Получаем сложность
            gameResultManager.ShowGameResult(message, difficulty); // Передаем сообщение и сложность
        }
    }

    private int GetDifficulty()
    {
        if (width == 5 && height == 5) return 5; // Легкая
        if (width == 10 && height == 10) return 10; // Средняя
        if (width == 15 && height == 15) return 15; // Сложная
        return 5; // По умолчанию легкая
    }

    private IEnumerable<Cell> GetNeighbourCells(Cell cell)
    {
        List<Cell> neighbourCells = new List<Cell>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector3Int neighbourCoords = new Vector3Int(cell.XCoord + i, cell.YCoord + j, 0);
                if (!positionToCell.ContainsKey(neighbourCoords)) continue;
                if (i == 0 && j == 0) continue;
                Cell neighbourCell = positionToCell[neighbourCoords];
                neighbourCells.Add(neighbourCell);
            }
        }
        return neighbourCells;
    }

    private int GetBombsAroundCell(Cell cell)
    {
        int bombsAround = 0;
        foreach (var neighbour in GetNeighbourCells(cell))
        {
            if (neighbour.IsBomb)
            {
                bombsAround++;
            }
        }
        return bombsAround;
    }

    private void OpenRandomEmptyCell()
    {
        bool isOpened = false;
        List<Cell> cellsToChooseFrom = new List<Cell>(cells);
        while (!isOpened && cellsToChooseFrom.Count > 0)
        {
            int randomIndex = Random.Range(0, cellsToChooseFrom.Count);
            Cell cell = cellsToChooseFrom[randomIndex];
            if (cell.IsBomb || GetBombsAroundCell(cell) != 0)
            {
                cellsToChooseFrom.Remove(cell);
                continue;
            }
            OpenCellByCoords(new Vector3Int(cell.XCoord, cell.YCoord, 0));
            isOpened = true;
        }
    }

    public void SetBombFlag(Vector3Int cellCoords)
    {
        Cell cell = positionToCell[cellCoords];
        SetBombFlagResult result = cell.SetBombFlag();
        if (result == SetBombFlagResult.Setted)
        {
            settedFlags++;
            visualizer.SetBombFlag(cell, result);
            if (cell.IsBomb)
            {
                remainedBombs--;
                if (remainedBombs == 0 && settedFlags == bombsToSetup)
                {
                    print("You win");
                }
            }
        }
        if (result == SetBombFlagResult.Unsetted)
        {
            visualizer.SetBombFlag(cell, result);
            settedFlags--;
            if (cell.IsBomb)
            {
                remainedBombs++;
            }
        }
    }
}