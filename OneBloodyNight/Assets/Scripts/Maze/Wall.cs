using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    //For the state of the wall; don't want to delete walls in case I need to use them to travel across cells.
    public enum wState
    {
        exterior,
        interior,
        destroyed
    }

    //For where the wall is located in the cell. NESW order
    public enum wLocation
    {
        north,
        east,
        south,
        west
    }

    //wall variables
    [SerializeField]
    [Tooltip("What is the state of the wall")]
    private wState state;
    [SerializeField]
    [Tooltip("Where is the wall")]
    private wLocation locate;
    [SerializeField]
    private Wall linked;
    //private Cell parent;

    //getter for the linked wall
    public Wall getLink()
    {
        if (linked == null)
        {
            //Debug.Log("Cell not linked on " + getCell() + "'s " + locate + " wall");
            return this;
        }
        return linked;
    }

    //getter for the state of the wall
    public wState getState()
    {
        return state;
    }

    //set the wall's state
    public void setState(wState s)
    {
        state = s;
    }

    //links two walls
    public void linkWall(Wall other)
    {
        if (other != null)
        {
            linked = other;
            state = wState.interior;
        } else
        {
            //pretty sure this'll give an error... Luckily it hasn't happened! :D
            Debug.Log("Wall " + name + " tried connecting to " + other.gameObject.name + "Which doesn't have component Wall.cs");
        }
    }

    //If the wall is hit by a projectile
    public void remove(bool extra)
    {
        if (state == wState.interior)
        {
            //disable & change the state of the wall
            state = wState.destroyed;
            gameObject.SetActive(false);
            if (!extra && linked != null)
            {
                linked.remove(true);
            }
        }
    }

    internal void placeNorthSprites(Biome b)
    {
        GameObject[] allSprites = Maze.m.biomeVariables[(int)b].North.Sprites;
        if (allSprites.Length >= 1)
        {
            int picked = Random.Range(0, allSprites.Length);
            GameObject temp = Instantiate(allSprites[picked], new Vector3(transform.position.x + Maze.m.biomeVariables[(int)b].North.offSet, transform.position.y, allSprites[picked].transform.position.z), Quaternion.identity, transform);
            picked = Random.Range(0, allSprites.Length);
            temp = Instantiate(allSprites[picked], new Vector3(transform.position.x - Maze.m.biomeVariables[(int)b].North.offSet, transform.position.y, allSprites[picked].transform.position.z), Quaternion.identity, transform);
        }
    }

    internal void placeEastSprites(Biome b)
    {
        GameObject[] allSprites = Maze.m.biomeVariables[(int)b].East.Sprites;
        if (allSprites.Length >= 1)
        {
            int picked = Random.Range(0, allSprites.Length);
            GameObject temp = Instantiate(allSprites[picked], new Vector3(transform.position.x, transform.position.y + Maze.m.biomeVariables[(int)b].East.offSet, allSprites[picked].transform.position.z), Quaternion.identity, transform);
            picked = Random.Range(0, allSprites.Length);
            temp = Instantiate(allSprites[picked], new Vector3(transform.position.x, transform.position.y - Maze.m.biomeVariables[(int)b].East.offSet, allSprites[picked].transform.position.z), Quaternion.identity, transform);
        }
    }

    public Cell getCell()
    {
        return transform.parent.gameObject.GetComponent<Cell>();
    }
}
