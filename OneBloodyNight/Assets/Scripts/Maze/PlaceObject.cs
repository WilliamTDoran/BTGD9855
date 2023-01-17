using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaceObject
{
    public static void massPlace(Biome b, PlacableObject obj)
    {
        int max = (int)Math.Floor(Random.Range(obj.minPlaced, obj.maxPlaced));
    }

    public static void place(Biome b, PlacableObject obj)
    {
        int x;
        int y;
        cell(b, obj.cellDifficulty, out x, out y);
        GameObject placed = GameObject.Instantiate(obj.formation, new Vector3(Maze.m.getWorldPosX(x), -Maze.m.getWorldPosX(y), obj.formation.transform.position.z), new Quaternion());
        //Debug.Log("Object placed in cell "+x+", "+y+", biome colour: "+Maze.m.getCell(x,y).getBiome());
        //Maze.m.getCell(x, y).transform.GetChild(0).gameObject.SetActive(true);
        Maze.m.getCell(x, y).transform.GetChild(0).GetComponent<Spawner>().addSpawnLocations(placed);
    }

    public static void cell(Biome b, float diff, out int x, out int y)
    {
        Cell c;
        do
        {
            x = Random.Range(0, Maze.m.width());
            y = Random.Range(0, Maze.m.height());
            c = Maze.m.getCell(x, y);
            // while the biomes don't match & the cell's difficulty is too high
        } while (c.getBiome() != b && diff+c.difficulty >= Maze.m.biomeVariables[(int)b].maxCellDifficulty);
    }
}
