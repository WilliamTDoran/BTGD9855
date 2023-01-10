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
    public void hit(bool extra)
    {
        if (state == wState.interior)
        {
            //disable & change the state of the wall
            state = wState.destroyed;
            gameObject.SetActive(false);
            if (!extra && linked != null)
            {
                linked.hit(true);
            }
        }
    }

    public Cell getCell()
    {
        return transform.parent.gameObject.GetComponent<Cell>();
    }
}
