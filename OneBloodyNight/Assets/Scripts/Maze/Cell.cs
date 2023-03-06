using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    //The 4 walls of the cell
    [SerializeField]
    [Tooltip("North, East, South, West order please.")]
    internal Wall[] walls;

    [SerializeField]
    private Biome biome = Biome.length;
    internal short difficulty;
    internal bool setPiece = false;
    //internal short placed;

    private bool inMaze = false;

    [SerializeField]
    private GameObject floor;

    internal Spawner spawner;

    void Awake()
    {
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

    public Biome getBiome()
    {
        return biome;
    }

    public void setBiome(Biome b)
    {
        biome = b;
    }

    /*void Update()
    {
        //floor.GetComponent<Renderer>().material.color = Maze.m.biomeVariables[(int)biome].colour;
    }*/

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
    public void generateMaze()
    {
        inMaze = true;
        difficulty = 0;
        //pick a cell around it.
        Cell nextCell;
        int dir;
        for (int j = 0; j < 4; j++) 
        {
            dir = (int) Random.Range(0, 4);
            nextCell = getWall(dir).getLink().getCell(); //Rigged to return itself if not possible
            //Make sure it's in the maze. If not select a new cell
            for (int i = 0; i < 4; i++)
            {
                if (!nextCell.isInMaze() && nextCell.getBiome() == biome)
                {
                    break;
                }
                dir++;
                dir %= 4;
                nextCell = getWall(dir).getLink().getCell();
            }
            //if all are, this is cell is done. go back up the generate maze loop
            if (nextCell.isInMaze() || nextCell.getBiome() != biome)
            {
                int openWalls = 0;
                for (int i = 0; i < walls.Length; i++)
                {
                    if (walls[i].getState() == Wall.wState.destroyed)
                    {
                        openWalls++;
                    }
                }
                if (openWalls <= 1) // if it's a dead end
                {
                    Maze.m.deadEnds.Add(this);
                }
                return;
            }
            //Connect the cell that's not connected
            walls[dir].remove(false);
            nextCell.generateMaze();
        }
        
        return;
    }

    internal void drawWalls()
    {
        //Debug.Log("Pos of "+gameObject.name + ": "+transform.position.x+" " + transform.position.y + " " + transform.position.z + " ");
        walls[(int)Wall.wLocation.north].placeNorthSprites(getBiome(), transform.position);
        walls[(int)Wall.wLocation.east].placeEastSprites(getBiome(), transform.position);

        //corners
        CornerType corner = Corner();
        if (Maze.m.biomeVariables[(int)getBiome()].Corner.Sprites.Length > (int)corner && Maze.m.biomeVariables[(int)getBiome()].Corner.Sprites[(int)corner] != null)
        {
            GameObject temp = Instantiate(Maze.m.biomeVariables[(int)getBiome()].Corner.Sprites[(int)corner], new Vector3(transform.position.x + Maze.m.biomeVariables[(int)getBiome()].Corner.offSet.x, transform.position.y, transform.position.z + Maze.m.biomeVariables[(int)getBiome()].Corner.offSet.y), Quaternion.Euler(90, 0, 0), transform);
        }
        if (walls[(int)Wall.wLocation.west].edge())
        {
            walls[(int)Wall.wLocation.west].placeEastSprites(getBiome(), transform.position);
        }
        if (walls[(int)Wall.wLocation.south].edge())
        {
            walls[(int)Wall.wLocation.south].placeNorthSprites(getBiome(), transform.position);
        }

        //floors
        GameObject[] floorSprites = Maze.m.biomeVariables[(int)getBiome()].floors.Sprites;
        for (int i=0; i<9; i++)
        {
            if (floorSprites.Length >= 1)
            {
                int picked = Random.Range(0, floorSprites.Length);
                Vector3 offset = Maze.m.biomeVariables[(int)getBiome()].floors.offSet;
                GameObject temp = Instantiate(floorSprites[picked], new Vector3(transform.position.x - offset.x + (i % 3) * offset.x, floorSprites[picked].transform.position.y, transform.position.z - offset.z + (i / 3) * offset.z), Quaternion.Euler(90, 0, 0), transform);
            }
        }
    }

    internal CornerType Corner()
    {
        bool[] intersection;
        intersection = new bool[4];
        intersection[3] = walls[(int)Wall.wLocation.north].getState() != Wall.wState.destroyed;
        intersection[2] = walls[(int)Wall.wLocation.east].getState() != Wall.wState.destroyed;
        intersection[0] = walls[(int)Wall.wLocation.north].getState() != Wall.wState.exterior && (walls[(int)Wall.wLocation.north].getLink().getCell().walls[(int)Wall.wLocation.east].getState() != Wall.wState.destroyed);
        intersection[1] = walls[(int)Wall.wLocation.east].getState() != Wall.wState.exterior && (walls[(int)Wall.wLocation.east].getLink().getCell().walls[(int)Wall.wLocation.north].getState() != Wall.wState.destroyed);
        if (intersection[0] && intersection[1] && intersection[2] && intersection[3]) return CornerType.All;
        if (!intersection[0] && intersection[1] && intersection[2] && intersection[3]) return CornerType.missingNorth;
        if (intersection[0] && !intersection[1] && intersection[2] && intersection[3]) return CornerType.missingEast;
        if (intersection[0] && intersection[1] && !intersection[2] && intersection[3]) return CornerType.missingSouth;
        if (intersection[0] && intersection[1] && intersection[2] && !intersection[3]) return CornerType.missingWest;
        if (intersection[0] && !intersection[1] && intersection[2] && !intersection[3]) return CornerType.lineVertical;
        if (!intersection[0] && intersection[1] && !intersection[2] && intersection[3]) return CornerType.lineHorizontal;
        if (intersection[0] && !intersection[1] && !intersection[2] && intersection[3]) return CornerType.cornerNorthWest;
        if (intersection[0] && intersection[1] && !intersection[2] && !intersection[3]) return CornerType.cornerNorthEast;
        if (!intersection[0] && intersection[1] && intersection[2] && !intersection[3]) return CornerType.cornerSouthEast;
        if (!intersection[0] && !intersection[1] && intersection[2] && intersection[3]) return CornerType.cornerSouthWest;
        if (intersection[0] && !intersection[1] && !intersection[2] && !intersection[3]) return CornerType.endNorth;
        if (!intersection[0] && intersection[1] && !intersection[2] && !intersection[3]) return CornerType.endEast;
        if (!intersection[0] && !intersection[1] && intersection[2] && !intersection[3]) return CornerType.endSouth;
        if (!intersection[0] && !intersection[1] && !intersection[2] && intersection[3]) return CornerType.endWest;
        return CornerType.None;
    }
}

/*
 * 
 *  *-*-*
 *  | | |
 *  *-*-*
 *  | | |
 *  *-*-*
 * 
 * */
