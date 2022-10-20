using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    //Maze modifiers
    [SerializeField]
    int width;
    [SerializeField]
    int height;
    [SerializeField]
    float scale = 1;

    [SerializeField]
    [Tooltip("-1 for random")]
    private int mazeSeed = -1;

    //Just to store the rows
    [SerializeField]
    GameObject sampleRow;

    BiomeGenerator biomeGen;

    //stores a a bunch of rows of cells
    Row[] rows;

    void Awake()
    {
        if (mazeSeed != -1)
        {
            Random.InitState(mazeSeed);
        }
    }

    //Hey! We need a maze here.
    void Start()
    {
        biomeGen = new BiomeGenerator(this);
        
        initMaze();
    }

    //Creates a grid of cells
    void initMaze()
    {
        //scales up the maze to scale size for easy conversion
        transform.localScale = new Vector3(scale, scale, 1);
        //create a bunch of rows
        rows = new Row[height];
        for (int i = 0; i < rows.Length; i++) 
        {
            GameObject temp = Instantiate(sampleRow, new Vector3(0, -1*i*scale, 0), Quaternion.identity, transform);
            temp.name = "Row " + i;
            rows[i] = temp.GetComponent<Row>();
            if (i > 0) 
            {
                rows[i].initRow(width, i, scale, rows[i-1]);
            } else
            {
                rows[i].initRow(width, i, scale);
            }
        }
        
        //runs the maze generation algorithm
        generateMaze();
    }

    public void generateMaze()
    {
        biomeGen.generateBiomes((int)width / 2, (int)height / 2);
        Cell mid = getCell((int)width / 2, (int)height / 2);
        mid.setBiome((BiomeGenerator.Biome)2);
        getCell((int)width / 2 + 1, (int)height / 2).generateMaze();
        getCell((int)width / 2 - 1, (int)height / 2).generateMaze();
    }

    //helper function, return cell (x,y) from the maze.
    public Cell getCell(int x, int y)
    {
        return rows[y].getCell(x);
    }


    public void Update()
    {
        if (Input.GetButtonDown("Debug Next"))
        {
            Debug.Log("New Maze time!");
            foreach (Row r in rows)
            {
                Destroy(r.gameObject);
            }
            initMaze();
            Debug.Log("New Maze Generated!");
        }
    }
}
