using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private int xCoord;
    private int yCoord;
    bool isBomb;
    bool isOpened = false;
    bool isFlagged = false;
    GameObject cellInstance;

    public Cell(int xCoord, int yCoord)
    {
        this.xCoord = xCoord;
        this.yCoord = yCoord;
    }

    public int XCoord { get => xCoord; }
    public int YCoord { get => yCoord; }
    public bool IsBomb { get => isBomb; set => isBomb = value; }
    public GameObject CellInstance { get => cellInstance; set => cellInstance = value; }

    public OpenCellResult OpenCell()
    {
        if (isOpened || isFlagged) return OpenCellResult.None;
        if (isBomb) return OpenCellResult.Gameover;
        isOpened = true;
        return OpenCellResult.Opened;
    }

    public SetBombFlagResult SetBombFlag()
    {
        if (isOpened) return SetBombFlagResult.None;
        if (isFlagged)
        {
            isFlagged = false;
            return SetBombFlagResult.Unsetted;
        }
        isFlagged = true;
        return SetBombFlagResult.Setted;
    }
}

public enum OpenCellResult
{
    Gameover,
    Opened,
    None
}

public enum SetBombFlagResult
{
    Setted,
    Unsetted,
    None
}