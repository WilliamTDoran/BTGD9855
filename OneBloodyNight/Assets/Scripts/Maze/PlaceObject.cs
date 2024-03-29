using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaceObject
{
    internal static void placePickups()
    {
        Cell c;
        for (int i = 0; i < Maze.m.width(); i++)
        {
            for (int j = 0; j < Maze.m.height(); j++)
            {
                c = Maze.m.getCell(i, j);
                if (c.getBiome() == Maze.m.traits.CharacterBiome || c.setPiece) continue;
                if (Random.Range(0, 1.0f) < Maze.m.traits.chanceObj)
                {
                    GameObject[] pickups = Maze.m.traits.pickupsObjects;
                    GameObject.Instantiate(pickups[Random.Range(0, pickups.Length)], c.transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(90, 0, 0), c.transform);
                }
            }
        }
    }


    internal static void placeDecore()
    {
        Cell c;
        for (int i=0; i<Maze.m.width(); i++)
        {
            for (int j=0; j<Maze.m.height(); j++)
            {
                c = Maze.m.getCell(i, j);
                //place 4 decore / cell
                BiomeVariables bv = Maze.m.biomeVariables[(int)c.getBiome()];
                if (bv.chanceObject >= Random.Range(0, 1.0f) && bv.objects.Length > 0)
                {
                    GameObject temp = GameObject.Instantiate(bv.objects[Random.Range(0, bv.objects.Length)], c.transform.position + new Vector3((0.25f + Random.Range(-0.1f, 0.1f)) * Maze.m.traits.scale, 0, (0.25f + Random.Range(-0.1f, 0.1f)) * Maze.m.traits.scale), Quaternion.Euler(90, 0, 0), c.transform);
                    temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y + 0.0001f * temp.transform.position.z, temp.transform.position.z);
                    //Debug.Log(temp.transform.localPosition.y+" "+ temp.transform.localPosition.z);
                }
                if (bv.chanceObject >= Random.Range(0, 1.0f) && bv.objects.Length > 0)
                {
                    GameObject temp = GameObject.Instantiate(bv.objects[Random.Range(0, bv.objects.Length)], c.transform.position + new Vector3((-0.25f + Random.Range(-0.1f, 0.1f)) * Maze.m.traits.scale, 0, (0.25f + Random.Range(-0.1f, 0.1f)) * Maze.m.traits.scale), Quaternion.Euler(90, 0, 0), c.transform);
                    temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y + 0.0001f * temp.transform.position.z, temp.transform.position.z);
                }
                if (bv.chanceObject >= Random.Range(0, 1.0f) && bv.objects.Length > 0)
                {
                    GameObject temp = GameObject.Instantiate(bv.objects[Random.Range(0, bv.objects.Length)], c.transform.position + new Vector3((0.25f + Random.Range(-0.1f, 0.1f)) * Maze.m.traits.scale, 0, (-0.25f + Random.Range(-0.1f, 0.1f)) * Maze.m.traits.scale), Quaternion.Euler(90, 0, 0), c.transform);
                    temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y + 0.0001f * temp.transform.position.z, temp.transform.position.z);
                }
                if (bv.chanceObject >= Random.Range(0, 1.0f) && bv.objects.Length > 0)
                {
                    GameObject temp = GameObject.Instantiate(bv.objects[Random.Range(0, bv.objects.Length)], c.transform.position + new Vector3((-0.25f + Random.Range(-0.1f, 0.1f)) * Maze.m.traits.scale, 0, (-0.25f + Random.Range(-0.1f, 0.1f)) * Maze.m.traits.scale), Quaternion.Euler(90, 0, 0), c.transform);
                    temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y + 0.0001f * temp.transform.position.z, temp.transform.position.z);
                }
            }
        }
    }

    internal static void placePortal(Biome B, int minDis)
    {
        //Maze.m.deadEnds;
        bool placed = false;
        Cell c;
        do
        {
            int i = Random.Range(0, Maze.m.deadEnds.Count);
            c = Maze.m.deadEnds[i];
            if (Vector3.Distance(c.transform.position, Maze.m.getCell((int)Mathf.Floor(Maze.m.width()*0.5f), (int)Mathf.Floor(Maze.m.height()*0.5f)).transform.position ) > minDis)
            {
                if (B == c.getBiome())
                {
                    Maze.m.deadEnds.RemoveAt(i);
                    placed = true;
                    c.setPiece = true;
                }
            }
            
        } while (!placed);
        GameObject temp = GameObject.Instantiate(Maze.m.biomeVariables[(int)B].bossPortal, new Vector3(c.transform.position.x,c.transform.position.y, c.transform.position.z), Quaternion.Euler(90, 0, 0), c.transform);
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
