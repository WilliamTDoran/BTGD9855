using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MazeVariables
{
    public int width;
    public int height;
    public float scale;
    public BiomeGenerator.Biome CharacterBiome;
}


public class Maze : MonoBehaviour
{
    internal static Maze m;

    [SerializeField]
    MazeVariables traits;

    [SerializeField]
    [Tooltip("-1 for random")]
    private int mazeSeed = -1;

    //Just to store the rows
    [SerializeField]
    GameObject sampleRow;

    [SerializeField]
    int toRemove = 5;

    [SerializeField]
    internal BiomeVariables[] biomeVariables;

    //[SerializeField]
    //public Color[] BIOMECOLOR = { new Color(1.00f, 0.30f, 0.22f, 1.0f), new Color(0, 1, 0), new Color(1, 0, 0), new Color(1, 1, 1) };

    BiomeGenerator biomeGen;

    //stores a a bunch of rows of cells
    Row[] rows;

    void Awake()
    {
        m = this;
        if (mazeSeed != -1)
        {
            Random.InitState(mazeSeed);
        } else
        {
            int seed = Random.Range(0, 2147483647);
            Debug.Log("The randomization seed is: "+seed);
            Random.InitState(seed);
        }
    }

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
        transform.localScale = new Vector3(traits.scale, traits.scale, 1);
        //create a bunch of rows
        rows = new Row[traits.height];
        for (int i = 0; i < rows.Length; i++) 
        {
            GameObject temp = Instantiate(sampleRow, new Vector3(0, -1*i* traits.scale, 0), Quaternion.identity, transform);
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
        transform.position = new Vector3(-1 * traits.scale * (traits.width/2), traits.scale * (traits.height/2), 0.001f);
    }

    public void generateMaze()
    {
        Cell mid = getCell((int)traits.width / 2, (int)traits.height / 2);
        mid.setBiome(traits.CharacterBiome);
        biomeGen.generateBiomes((int)traits.width / 2, (int)traits.height / 2);
        getCell((int)traits.width / 2 + 1, (int)traits.height / 2).generateMaze();
        getCell((int)traits.width / 2 - 1, (int)traits.height / 2).generateMaze();
        mid.getWall((int)Wall.wLocation.east).hit(false);
        mid.getWall((int)Wall.wLocation.west).hit(false);
        extraRemoval(toRemove);
    }

    //helper function, return cell (x,y) from the maze.
    public Cell getCell(int x, int y)
    {
        return rows[y].getCell(x);
    }

    public void extraRemoval(int toRemove)
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
    }


    public void Update()
    {
        /*if (Input.GetButtonDown("Debug Next"))
        {
            //Debug.Log("New Maze time!");
            foreach (Row r in rows)
            {
                Destroy(r.gameObject);
            }
            initMaze();
            //Debug.Log("New Maze Generated!");
        }*/
    }
}
