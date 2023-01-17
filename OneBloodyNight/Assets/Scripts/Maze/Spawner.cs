using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    GameObject[] spawns;

    private void Start()
    {
        if (transform.parent == null) Debug.Log("Spawner not set to child of anything. Please child it to a cell.");
        Cell c = transform.parent.GetComponent<Cell>();
        if (c == null)
        {
            Debug.Log("Spawner error -> parent of not a cell!");
        } else
        {
            c.spawner = this;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Loaded")
        {
            //Load spawns in
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Loaded")
        {
            //unload spawns in
        }
    }
}
