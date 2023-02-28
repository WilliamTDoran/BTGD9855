using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Maze : MonoBehaviour
{
    internal static Maze m;

    [SerializeField]
    internal MazeVariables traits;

    [SerializeField]
    [Tooltip("-1 for random")]
    private int mazeSeed = -1;

    //Just to store the rows
    [SerializeField]
    GameObject sampleRow;

    [SerializeField]
    internal BiomeVariables[] biomeVariables;
    internal int width() { return traits.width; }
    internal int height() { return traits.height; }


    [HideInInspector]
    internal List<Cell> deadEnds;

    BiomeGenerator biomeGen;

    Row[] rows;

    void Awake()
    {
        m = this;
        seedRand();
    }

    void seedRand()
    {
        if (mazeSeed != -1)
        {
            Random.InitState(mazeSeed);
        }
        else
        {
            int seed = Random.Range(0, 2147483647);
            Debug.Log("The randomization seed is: " + seed);
            Random.InitState(seed);
        }
    }

    /*internal int getWorldPosX(int x)
    {
        return (int)Mathf.Round(-1 * traits.scale * (traits.width / 2) + x * traits.scale);
    }
    internal int getWorldPosY(int y)
    {
        return -getWorldPosX(y);
    }*/

    //Hey! We need a maze here.
    void Start()
    {
        biomeGen = new BiomeGenerator();

        initMaze();
    }

    //Creates a grid of cells
    void initMaze()
    {
        //scales up the maze to scale size for easy conversion
        transform.localScale = new Vector3(traits.scale, 1, traits.scale);
        //create a bunch of rows
        rows = new Row[traits.height];
        deadEnds = new List<Cell>();
        for (int i = 0; i < rows.Length; i++) 
        {
            GameObject temp = Instantiate(sampleRow, transform.position+new Vector3(transform.position.x, 0, -1 * i * traits.scale - traits.betweenCells * i), Quaternion.identity, transform);
            temp.name = "Row " + i;
            rows[i] = temp.GetComponent<Row>();
            if (i > 0) 
            {
                rows[i].initRow(traits.width, i, traits.scale, rows[i-1]);
            } else
            {
                rows[i].initRow(traits.width, i, traits.scale);
            }
        }
        
        //runs the maze generation algorithm
        generateMaze();
        transform.position = new Vector3(-1 * traits.scale * (traits.width / 2), 0.001f, traits.scale * (traits.height / 2));
        Player.plr.transform.position = new Vector3(getCell(traits.width/2, traits.height/ 2).transform.position.x, Player.plr.transform.position.y, getCell(traits.width / 2, traits.height / 2).transform.position.z);
    }

    public void generateMaze()
    {
        //Biomes
        Cell mid = getCell((int)traits.width / 2, (int)traits.height / 2);
        mid.setBiome(traits.CharacterBiome);
        biomeGen.generateBiomes((int)traits.width / 2, (int)traits.height / 2);

        //Maze generation
        getCell((int)traits.width / 2 + 1, (int)traits.height / 2).generateMaze();
        getCell((int)traits.width / 2 - 1, (int)traits.height / 2).generateMaze();
        mid.getWall((int)Wall.wLocation.east).remove(false);
        mid.getWall((int)Wall.wLocation.west).remove(false);

        //Wall removal
        //extraRemoval(toRemove);
        Debug.Log("There are: " + deadEnds.Count + "Dead ends");
        MazeWallRemoval removal = new MazeWallRemoval();
        removal.removeWalls(Biome.impundulu);
        removal.removeWalls(Biome.yara);

        for (int i=0; i<width(); i++)
        { 
            for (int j = 0; j < height(); j++)
            {
                getCell(i,j).drawWalls();
            }
        }

        //Object Placer
        //PlaceObject.place(Biome.yara, biomeVariables[(int)Biome.yara].objects[0]);
        PlaceObject.placeAll(traits.CharacterBiome);
    }

    //helper function, return cell (x,y) from the maze.
    public Cell getCell(int x, int y)
    {
        return rows[y].getCell(x);
    }

    /*public void extraRemoval(int toRemove)
    {
        for (int i=0; i<toRemove; i++)
        {
            Cell c = getCell(Random.Range(0, traits.width), Random.Range(0, traits.height));
            int wallRemoved = Random.Range(0, 4);
            Wall w = c.getWall(wallRemoved);
            Cell other = w.getLink().getCell();
            if (c.getBiome() == other.getBiome() && c != other)
            {
                w.hit(false);
            } 
        }
    }*/

    internal void clearVars()
    {
        rows = null;
    }

    public void RegenMaze()
    {
        Debug.Log("New Maze time!");
        if (rows != null)
        {
            foreach (Row r in rows)
            {
                DestroyImmediate(r.gameObject);
            }
        }
        if (biomeGen == null)
        {
            biomeGen = new BiomeGenerator();
        }
        initMaze();
        Debug.Log("New Maze Generated!");
    }


    public void Update()
    {
        if (Input.GetButtonDown("RegenerateMaze"))
        {
            RegenMaze();
        }
    }
}
