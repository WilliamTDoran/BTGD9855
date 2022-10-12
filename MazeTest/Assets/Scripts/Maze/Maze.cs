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

    //Just to store the rows
    [SerializeField]
    GameObject sampleRow;

    private IEnumerator generation1;


    public enum runningGenerator
    {
        Green,
        Red,
        length
    }

    public runningGenerator running = runningGenerator.Green;

    //stores a a bunch of rows of cells
    Row[] rows;

    //Hey! We need a maze here.
    void Start()
    {
        //GameObject.Find("Floor").transform.localScale = new Vector3(scale*5*width,height*scale*5, 1);
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

        generateMaze();
    }

    public void generateMaze()
    {
        generation1 = coRunning();
        StartCoroutine(generation1);
        getCell((int)width / 2 - 1, (int)height / 2).generateMaze(new Color(1, 0, 0, 1), runningGenerator.Red);
    }

    private IEnumerator coRunning()
    {
        getCell((int)width - 1, (int)height - 1).generateMaze(new Color(0, 1, 0, 1), runningGenerator.Green);
        yield return null;
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
