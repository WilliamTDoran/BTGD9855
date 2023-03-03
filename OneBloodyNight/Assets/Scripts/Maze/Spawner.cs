using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Spawn[] spawns;
    private bool spawned = false;
    [SerializeField]
    private int timeStartSpawning;
    [SerializeField]
    private float minSpawningDistance;
    [SerializeField]
    private float maxSpawningDistance;
    private float plrDistance;
    [SerializeField]
    private spawnerVars vars;
    private static int numEnem = 0;

    internal static void enemKilled() { numEnem--; }

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

        StartCoroutine(startSpawning());
    }

    /// <summary>
    /// Coroutine to not spawn enemies as soon as scene starts
    /// </summary>
    /// <returns></returns>
    private IEnumerator startSpawning()
    {
        spawned = true;
        yield return new WaitForSeconds(timeStartSpawning + Random.Range(0,1.5f));
        spawned = false;
        StartCoroutine(spawnAttempt());
    }

    /// <summary>
    /// Coroutine to attempt to spawn enemies
    /// </summary>
    /// <returns></returns>
    private IEnumerator spawnAttempt()
    {
        while(true)
        {
            spawnCheck();
            yield return new WaitForSeconds(2);
        }
    }

    /// <summary>
    /// Function to check all spawning conditions
    /// </summary>
    private void spawnCheck()
    {
        if (!spawned)
        {
            plrDistance = Vector3.Distance(Player.plr.transform.position, transform.position);
            if (plrDistance < maxSpawningDistance && plrDistance > minSpawningDistance)
            {
                int attempts = Random.Range(vars.minAttempts, vars.maxAttempts);
                for (int i=0; i<attempts; i++)
                {
                    float spawnChance = 100 * (Mathf.Pow(Bloodmeter.instance.bloodmeter.value/ Bloodmeter.instance.bloodmeter.maxValue, vars.baseChance) * Mathf.Pow(1 - numEnem/vars.maxCount, vars.steepness));
                    if (spawnChance > Random.Range(0, 100.0f))
                    {
                        StartCoroutine(spawnEnemy());
                    }
                    //start spawning coroutine for enemy offset
                }
                spawned = true;
            }
        }
    }

    /// <summary>
    /// spawns all enemies on a delay
    /// </summary>
    /// <returns></returns>
    private IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(Random.Range(0, 0.1f));
        plrDistance = Vector3.Distance(Player.plr.transform.position, transform.position);
        if (plrDistance > minSpawningDistance)
        {
            numEnem++;
            //spawn enemy
            Cell c = transform.parent.GetComponent<Cell>();
            GameObject[] enemies = Maze.m.biomeVariables[(int)c.getBiome()].enemies;
            int enem = Random.Range(0, enemies.Length);
            Debug.Log("Spawning enemy number " + enem +" in cell "+ c.name);
            GameObject placed = GameObject.Instantiate(enemies[enem], /*Maze.m.transform.position + */new Vector3(c.transform.position.x, c.transform.position.y, c.transform.position.z), Quaternion.Euler(90, 0, 0));
            Debug.Log("Enemy spawned at "+ placed.transform.position.x+" " + placed.transform.position.y + " " + placed.transform.position.z + " ");
        }
    }

    

    /*
    internal void addSpawnLocations(GameObject obj)
    {
        if (spawns == null) spawns = new Spawn[0];
        //Debug.Log("Adding Object");
        obj.transform.parent = transform;
        for(int i=0; i < obj.transform.childCount; i++)
        {
            //Debug.Log("Pass "+i);
            Transform c = obj.transform.GetChild(i);
            Spawn[] s = new Spawn[spawns.Length + 1];
            for (int j = 0; j<spawns.Length; j++)
            {
                s[j] = spawns[j];
            }
            s[spawns.Length] = new Spawn((int)c.position.x, (int)c.position.z, c.gameObject);
            spawns = s;
        }

        //unload();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            load();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            unload();
        }
    }

    private void load()
    {
        //Debug.Log("Collision enter! " + other.gameObject.name);
        //Debug.Log("in");
        if (spawns == null)
        {
            return;
        }
        //Load spawns in
        foreach (Spawn s in spawns)
        {
            //Debug.Log(s.obj.name + " just moved from " + s.obj.transform.position.x + ", " + s.obj.transform.position.y + " to ");
            //s.obj.transform.position = new Vector3(s.x - Maze.m.width() / 2.0f * Maze.m.traits.scale, s.y + 2 * Maze.m.traits.scale, s.obj.transform.position.z);
            if (!s.obj.gameObject.activeInHierarchy)
            {
                s.obj.transform.position = new Vector3(s.x, s.y, 0);
            }
            //Debug.Log(s.obj.transform.position.x + ", " + s.obj.transform.position.y + " to ");
            s.obj.SetActive(true);
        }
    }


    private void unload()
    {
        //Debug.Log("Collision exit! " + other.gameObject.name);
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
    }*/
}
