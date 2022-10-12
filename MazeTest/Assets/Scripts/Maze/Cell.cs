using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    //The 4 walls of the cell
    [SerializeField]
    [Tooltip("North, East, South, West order please.")]
    private Wall[] walls;

    private Maze m;

    [SerializeField]
    private Color cellColour;
    //int i = 0;

    private bool inMaze = false;

    [SerializeField]
    private GameObject floor;

    void Awake()
    {
        m = GameObject.FindGameObjectsWithTag("Maze")[0].GetComponent<Maze>();
    }

    //hey, what wall you need?
    public Wall getWall(int w)
    {
        if (w >= 0 && w < walls.Length) 
        {
            return walls[w];
        } else
        {
            Debug.Log("Index doesn't exist on cell " + gameObject.name);
            return null;
        }
    }

    void Update()
    {
        floor.GetComponent<Renderer>().material.color = cellColour;
    }

    public bool isInMaze()
    {
        return inMaze;
    }

    //helper function to help setup the maze. Connects this cell to the cell on the left
    public void leftCell(Cell l)
    {
        if (l != null)
        {
            connect(walls[(int)Wall.wLocation.west], l.getWall((int)Wall.wLocation.east));
        }
    }

    //helper function help setup the maze. Connects this cell to the cell above
    public void topCell(Cell t)
    {
        if (t != null)
        {
            connect(walls[(int)Wall.wLocation.north], t.getWall((int)Wall.wLocation.south));
        }
    }

    //helper function. links two walls
    private void connect (Wall here, Wall there)
    {
        //Debug.Log("Connecting cells." + here.name + " and " + there.name);
        here.linkWall(there);
        there.linkWall(here);
    }


    //The main loop of the maze Algorithm
    //Note to self: later may need to make into coroutine
    public void generateMaze(Color c, Maze.runningGenerator thisGen)
    {
        inMaze = true;
        //floor.GetComponent<Renderer>().material.color = c;
        cellColour = c;
        //swap function running
        int temp = (int) (m.running + 1) % (int)Maze.runningGenerator.length;
        m.running = (Maze.runningGenerator)temp;
        /*Debug.Log(m.running + " from " + c+", pre loop");
        temp = 0;
        while (m.running != thisGen)
        {
            temp++;
            //Debug.Log(m.running + " from " + c + ", loop iteration " + temp);
            if (temp >= 1000) 
            {
                break;
            }
        }*/
        //Debug.Log("Cell " + gameObject.name + " is being connected to the maze");
        //pick a cell around it.
        Cell nextCell;
        int dir;
        for (int j = 0; j < 4; j++) 
        {
            dir = (int)Random.Range(0, 4);
            nextCell = getWall(dir).getLink().getCell(); //Rigged to return itself if not possible
            //Make sure it's in the maze. If not select a new cell
            for (int i = 0; i < 4; i++)
            {
                if (!nextCell.isInMaze())
                {
                    break;
                }
                dir++;
                dir %= 4;
                nextCell = getWall(dir).getLink().getCell();
            }
            //if all are, this is a deadend. go back up the generate maze loop
            if (nextCell.isInMaze())
            {
                return;
            }
            //Connect the cell that's not connected
            walls[dir].hit(false);
            nextCell.generateMaze(c, thisGen);
        }
        return;
    }
}
