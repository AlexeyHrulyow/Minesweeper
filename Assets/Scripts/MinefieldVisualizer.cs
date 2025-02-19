using System;
using System.Collections.Generic;
using UnityEngine;

public class MinefieldVisualizer : MonoBehaviour
{
    [SerializeField] GameObject closedCell;
    [SerializeField] Transform cellContainer;
    [SerializeField] DigitSprites[] digitsprite;
    [SerializeField] Sprite flagSprite;
    [SerializeField] Sprite closedSprite;

    public void VisualizeCellsOnStart(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            GameObject cellInstance = Instantiate(closedCell, new Vector3(cell.XCoord, cell.YCoord, 0), Quaternion.identity);

            cellInstance.transform.parent = cellContainer;
            cell.CellInstance = cellInstance;
        }
    }

    public void OpenCell(Cell cell, int bombsAround)
    {
        cell.CellInstance.GetComponent<SpriteRenderer>().sprite = GetBombsAroundSprite(bombsAround);
    }

    private Sprite GetBombsAroundSprite(int bombsAround)
    {
        foreach (var sprite in digitsprite)
        {
            if (sprite.numberOfBombs == bombsAround) return sprite.digitSprite;
        }
        return null;
    }

    public void SetBombFlag(Cell cell, SetBombFlagResult result)
    {
        if (result == SetBombFlagResult.Setted)
        {
            cell.CellInstance.GetComponent<SpriteRenderer>().sprite = flagSprite;
        }
        else
        {
            cell.CellInstance.GetComponent<SpriteRenderer>().sprite = closedSprite;
        }
    }
}

[System.Serializable]
public class DigitSprites
{
    public int numberOfBombs;
    public Sprite digitSprite;


}