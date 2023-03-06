using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaceObject
{

    /*public static void placeAll(Biome startBiome)
    {
        BiomeVariables[] bv = Maze.m.biomeVariables;
        for (int b = 0; b<(int)Biome.length; b++)
        {
            if (b == (int)startBiome) continue;
            for (int i=0; i < bv[b].objects.Length; i++)
            {
                massPlace((Biome)b, bv[b].objects[i]);
            }
        }
    }

    public static void massPlace(Biome b, PlacableObject obj)
    {
        int max = (int)Math.Floor(Random.Range(obj.minPlaced, obj.maxPlaced));
        for(int i=0; i<max; i++)
        {
            place(b, obj);
        }
    }

    public static void place(Biome b, PlacableObject obj)
    {
        int x;
        int y;
        cell(b, obj.cellDifficulty, out x, out y);
        Cell c = Maze.m.getCell(x, y);
        GameObject placed = GameObject.Instantiate(obj.formation, Maze.m.transform.position+new Vector3(c.transform.position.x + Random.Range(-Maze.m.traits.scale*0.4f, Maze.m.traits.scale * 0.4f), c.transform.position.y, c.transform.position.z), Quaternion.Euler(90, 0, 0));
        //Debug.Log("Object placed in cell "+x+", "+y+", biome colour: "+Maze.m.getCell(x,y).getBiome());
        //Maze.m.getCell(x, y).transform.GetChild(0).gameObject.SetActive(true);
        //Maze.m.getCell(x, y).transform.GetChild(0).GetComponent<Spawner>().addSpawnLocations(placed);
    }*/

    internal static void placePortal(Biome B)
    {
        //Maze.m.deadEnds;
        bool placed = false;
        Cell c;
        do
        {
            int i = Random.Range(0, Maze.m.deadEnds.Count);
            c = Maze.m.deadEnds[i];
            if (B == c.getBiome()) {
                Maze.m.deadEnds.RemoveAt(i);
                placed = true;
                c.setPiece = true;
            }
        } while (!placed);
        GameObject temp = GameObject.Instantiate(Maze.m.traits.bossPortal, new Vector3(c.transform.position.x,c.transform.position.y, c.transform.position.z), Quaternion.Euler(90, 0, 0), c.transform);
    }

    /*public static void cell(Biome b, float diff, out int x, out int y)
    {
        Cell c;
        do
        {
            x = Random.Range(0, Maze.m.width());
            y = Random.Range(0, Maze.m.height());
            c = Maze.m.getCell(x, y);
            // while the biomes don't match & the cell's difficulty is too high
        } while (c.getBiome() != b && diff+c.difficulty >= Maze.m.biomeVariables[(int)b].maxCellDifficulty);
    }*/
}
