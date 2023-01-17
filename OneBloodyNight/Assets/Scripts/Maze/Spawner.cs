using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Spawn[] spawns;

    private void Awake()
    {
        //gameObject.SetActive(false);
    }

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

    internal void addSpawnLocations(GameObject obj)
    {
        if (spawns == null)
        {
            spawns = new Spawn[0];
        }
        Debug.Log("Adding Object");
        for(int i=0; i < obj.transform.childCount; i++)
        {
            Debug.Log("Pass "+i);
            Transform c = obj.transform.GetChild(i);
            obj.transform.SetParent(transform, true);
            Spawn[] s = new Spawn[spawns.Length + 1];
            for (int j = 0; j<spawns.Length; j++)
            {
                s[j] = spawns[j];
            }
            s[spawns.Length] = new Spawn((int)c.position.x, (int)c.position.y, c.gameObject);
            spawns = s;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision enter! " + other.gameObject.name);
        if (other.tag == "Loaded")
        {
            //Debug.Log("in");
            if (spawns == null)
            {
                return;
            }
            //Load spawns in
            foreach (Spawn s in spawns)
            {
                s.obj.transform.position = new Vector3(s.x, s.y, s.obj.transform.position.z);
                s.obj.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Collision exit! " + other.gameObject.name);
        if (other.tag == "Loaded")
        {
            //Debug.Log("in");
            if (spawns == null)
            {
                return;
            }
            //unload spawns in
            foreach (Spawn s in spawns)
            {
                //s.obj.transform.position = new Vector3(s.x, s.y, s.obj.transform.position.z);
                s.obj.SetActive(false);
            }
        }
    }
}
