using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Maze : MonoBehaviour
{
    //Static reference for maze
    internal static Maze m;
    

    //Variables!!!
    [Header("Seeding the Maze")]
    [SerializeField]
    [Tooltip("What seed within the list the maze will start at. -1 for a random seed")]
    private int currSeed;
    [SerializeField]
    [Tooltip("List of seeds the maze will pick, starting at the current seed")]
    private int[] mazeSeed;


    [Header("Groups of Variables")]
    [SerializeField]
    [Tooltip("Variables for the entire maze")]
    internal MazeVariables traits;


    [SerializeField]
    [Tooltip("Variables for the seperate biomes of the maze")]
    internal BiomeVariables[] biomeVariables;


    //Just to store the rows
    [Header("Prefabs")]
    [SerializeField]
    [Tooltip("Drag the Row Prefab here!")]
    GameObject sampleRow;


    //width. Just how many cells across the maze is.
    internal int width() { return traits.width; }
    //Height. How tall is the maze in cells?
    internal int height() { return traits.height; }


    //Variable storing all the dead ends. Used for Portal spawning and wall removal
    [HideInInspector]
    internal List<Cell> deadEnds;


    //Reference for the biome generator script
    BiomeGenerator biomeGen;


    //Contains the rows for the maze
    Row[] rows;


    /// <summary>
    /// Sets stuff up. Must be called before even start starts.
    /// </summary>
    void Awake()
    {
        m = this;
        seedRand();
    }


    /// <summary>
    /// Function that seeds the randomness if it's been programmed in
    /// </summary>
    void seedRand()
    {
        if (mazeSeed.Length != 0 && currSeed != -1)
        {
            Random.InitState(mazeSeed[currSeed]);
            currSeed = (currSeed + 1) % mazeSeed.Length;
        }
        else
        {
            int seed = Random.Range(0, 2147483647);
            Debug.Log("The randomization seed is: " + seed);
            Random.InitState(seed);
        }
    }


    /// <summary>
    /// Hey! We need a maze here.
    /// </summary>
    void Start()
    {
        biomeGen = new BiomeGenerator();

        initMaze();
    }


    /// <summary>
    /// Sets up everything needed to generate a maze
    /// </summary>
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

        //aligns maze & sets up hub 
        transform.position = new Vector3(-1 * traits.scale * (traits.width / 2), 0.001f, traits.scale * (traits.height / 2));
        Vector3 hub = new Vector3(getCell(traits.width / 2, traits.height / 2).transform.position.x, Player.plr.transform.position.y, getCell(traits.width / 2, traits.height / 2).transform.position.z);
        Player.plr.transform.position = hub;
        for (int i=0; i<traits.hubObjects.Length; i++)
        {
            GameObject temp = GameObject.Instantiate(Maze.m.traits.hubObjects[i], hub + Maze.m.traits.hubObjects[i].transform.position, Quaternion.Euler(90, 0, 0), getCell(traits.width / 2, traits.height / 2).transform);
        }
    }


    /// <summary>
    /// Generates a new maze. Maze sure initMaze() is called at directly prior
    /// </summary>
    private void generateMaze()
    {
        //Biomes
        Cell mid = getCell((int)traits.width / 2, (int)traits.height / 2);
        for (int i=-1; i<=1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Cell c = getCell((int)traits.width / 2 + i, (int)traits.height / 2 + j);
                c.setBiome(traits.CharacterBiome);
                c.setPiece = true;
            }
        }
        biomeGen.generateBiomes((int)traits.width / 2, (int)traits.height / 2);

        //Set pieces
        LocationSpawner.placeLocation((Biome)((((int)mid.getBiome()) + 1) % 3), 3);
        LocationSpawner.placeLocation((Biome)((((int)mid.getBiome()) + 2) % 3), 3);

        //Maze generation
        getCell((int)traits.width / 2 + 2, (int)traits.height / 2).generateMaze();
        getCell((int)traits.width / 2 - 2, (int)traits.height / 2).generateMaze();
        mid.getWall((int)Wall.wLocation.east).remove(false);
        mid.getWall((int)Wall.wLocation.west).remove(false);
        mid.getWall((int)Wall.wLocation.north).remove(false);
        mid.getWall((int)Wall.wLocation.south).remove(false);
        getCell((int)traits.width / 2 + 1, (int)traits.height / 2).getWall((int)Wall.wLocation.north).remove(false);
        getCell((int)traits.width / 2 + 1, (int)traits.height / 2).getWall((int)Wall.wLocation.south).remove(false);
        getCell((int)traits.width / 2 + 1, (int)traits.height / 2).getWall((int)Wall.wLocation.east).remove(false);
        getCell((int)traits.width / 2 - 1, (int)traits.height / 2).getWall((int)Wall.wLocation.north).remove(false);
        getCell((int)traits.width / 2 - 1, (int)traits.height / 2).getWall((int)Wall.wLocation.south).remove(false);
        getCell((int)traits.width / 2 - 1, (int)traits.height / 2).getWall((int)Wall.wLocation.west).remove(false);
        getCell((int)traits.width / 2, (int)traits.height / 2 + 1).getWall((int)Wall.wLocation.east).remove(false);
        getCell((int)traits.width / 2, (int)traits.height / 2 + 1).getWall((int)Wall.wLocation.west).remove(false);
        getCell((int)traits.width / 2, (int)traits.height / 2 - 1).getWall((int)Wall.wLocation.east).remove(false);
        getCell((int)traits.width / 2, (int)traits.height / 2 - 1).getWall((int)Wall.wLocation.west).remove(false);

        //Boss portals
        int y = PlayerPrefs.GetInt("Yara");
        if (y != 0)
        {
            PlaceObject.placePortal((Biome)((((int)mid.getBiome()) + 1) % 3), 60);
        }
        y = PlayerPrefs.GetInt("Imp");
        if (y != 0)
        {
            PlaceObject.placePortal((Biome)((((int)mid.getBiome()) + 2) % 3), 60);
        }

        //Wall removal
        MazeWallRemoval removal = new MazeWallRemoval();
        removal.removeWalls((Biome)((((int)mid.getBiome()) + 1) % 3));
        removal.removeWalls((Biome)((((int)mid.getBiome()) + 2) % 3));
        

        //Spawn wall art in
        for (int i=0; i<width(); i++)
        { 
            for (int j = 0; j < height(); j++)
            {
                getCell(i,j).drawWalls();
            }
        }

        //Object Placer
        PlaceObject.placeDecore();
        PlaceObject.placePickups();
    }


    /// <summary>
    /// helper function, return cell (x,y) from the maze.
    /// </summary>
    /// <param name="x">The x position of the cell you want in the maze.</param>
    /// <param name="y">The y position of the cell you want in the maze.</param>
    /// <returns></returns>
    internal Cell getCell(int x, int y)
    {
        return rows[y].getCell(x);
    }


    /// <summary>
    /// Called when the G button is pressed. Destroys the old maze, then creates a new maze.
    /// </summary>
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
        seedRand();
        initMaze();
        Debug.Log("New Maze Generated!");
    }

    /*
    /// <summary>
    /// Update. Only used for maze regeneration
    /// </summary>
    public void Update()
    {
        if (Input.GetButtonDown("RegenerateMaze"))
        {
            RegenMaze();
        }
    }*/
}
