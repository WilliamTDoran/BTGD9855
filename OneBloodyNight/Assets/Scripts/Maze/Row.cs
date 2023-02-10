using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    //Info about this rows
    private int width;
    private int y; //how low down the maze this row is. Top=0, bottom=Maze.height;
    private float scale;
    private Cell[] cells;

    //What is created
    [SerializeField]
    private GameObject sampleCell;

    //heper function. Gets a cell from a row
    public Cell getCell(int x)
    {
        return cells[x];
    }

    //initiate the top row. (For no array out of bounds)
    public void initRow(int w, int h, float s)
    {
        initRow(w, h, s, null);
    }

    //initiate the row. (for grid of cells)
    public void initRow(int w, int h, float s, Row above)
    {
        //assign variable values
        y = h;
        width = w;
        scale = s;
        //initiate array of cells
        cells = new Cell[width];
        for (int i = 0; i < cells.Length; i++) 
        {
            GameObject temp = Instantiate(sampleCell, transform.position + new Vector3(i*scale+Maze.m.traits.betweenCells * i, 0, 0), Quaternion.identity, transform);
            temp.name = "Cell (" + i + "," + y + ")";
            cells[i] = temp.GetComponent<Cell>();
            if (i > 0) 
            {
                cells[i].leftCell(cells[i - 1]);
            }
            if (y > 0)
            {
                cells[i].topCell(above.getCell(i));
            }
        }
    }
}
