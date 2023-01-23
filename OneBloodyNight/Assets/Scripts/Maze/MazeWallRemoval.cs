using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWallRemoval
{
    private List<Cell> deadEnds;

    internal MazeWallRemoval()
    {
        if (Maze.m != null)
        {
            deadEnds = Maze.m.deadEnds;
        }
    }

    internal void removeWalls(Biome b) // replace deadEnds with match & match biome
    {
        BiomeVariables traits = Maze.m.biomeVariables[(int)b];
        List<Cell> match = new List<Cell>();
        foreach (Cell c in deadEnds)
        {
            if (c.getBiome() == b) match.Add(c); 
        }
        Debug.Log("Biome "+b+" contains "+match.Count+" dead ends");

        for (int i=0; i<traits.wallsRemoved; i++)
        {
            int matchChosen = Random.Range(0, match.Count);
            int maxAvailable = 0;
            for (int j=0; j<match[matchChosen].walls.Length; j++)
            {
                if (match[matchChosen].walls[j].getState() == Wall.wState.interior)
                {
                    maxAvailable++;
                }
            }
            if (maxAvailable > 0)
            {
                maxAvailable = Random.Range(0, maxAvailable);
                Wall w = null;
                for (int j = 0; j < match[matchChosen].walls.Length; j++)
                {
                    if (match[matchChosen].walls[j].getState() != Wall.wState.interior) continue;
                    maxAvailable--;
                    if (maxAvailable == 0)
                    {
                        w = match[matchChosen].walls[j];
                        Debug.Log("Cell "+match[matchChosen].name+" removed the "+w.name);
                        break;
                    }

                }
                if (w != null)
                {
                    w.remove(false);
                }
            }
        }
    }
}
