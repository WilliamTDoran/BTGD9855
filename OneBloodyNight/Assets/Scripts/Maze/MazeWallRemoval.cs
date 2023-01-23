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

    internal void removeWalls(Biome b)
    {
        BiomeVariables traits = Maze.m.biomeVariables[(int)b];
        List<Cell> match = new List<Cell>();
        foreach (Cell c in deadEnds)
        {
            if (c.getBiome() == b) match.Add(c); 
        }
        Debug.Log("Biome "+b+" contains "+match.Count+" dead ends");
    }
}
