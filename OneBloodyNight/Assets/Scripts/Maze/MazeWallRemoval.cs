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
        //Debug.Log("Biome "+b+" contains "+match.Count+" dead ends");
        for (int i=0; i<traits.wallsRemoved; i++)
        {
            if (match.Count > 1)
            {
                int matchChosen = Random.Range(1, match.Count) - 1;
                match.RemoveAt(matchChosen);
                int maxAvailable = 0;
                //Debug.Log(matchChosen+ ", "+ match.Count);
                for (int j = 1; j < match[matchChosen].walls.Length; j++)
                {
                    if (match[matchChosen].walls[j].getState() != Wall.wState.interior && //if the wall's not an interior wall
                        match[matchChosen].getBiome() == match[matchChosen].walls[j].getLink().getCell().getBiome() && //if the biomes match
                        !match[matchChosen].walls[j].getLink().getCell().setPiece && //if the connected cell is not a setpiece
                        !match[matchChosen].setPiece) //if this cell is not a setpiece
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
                        if (match[matchChosen].walls[j].getState() != Wall.wState.interior 
                            && match[matchChosen].getBiome() == match[matchChosen].walls[j].getLink().getCell().getBiome() && 
                            !match[matchChosen].walls[j].getLink().getCell().setPiece && 
                            !match[matchChosen].setPiece) continue;
                        maxAvailable--;
                        if (maxAvailable == 0)
                        {
                            w = match[matchChosen].walls[j];
                            //Debug.Log("Cell " + match[matchChosen].name + " removed the " + w.name +" wall");
                            break;
                        }

                    }
                    if (w != null)
                    {
                        w.remove(false);
                    }
                }
            } else
            {
                Cell c = Maze.m.getCell(Random.Range(0, Maze.m.width()), Random.Range(0, Maze.m.height()));
                int wallRemoved = Random.Range(0, 4);
                Wall w = c.getWall(wallRemoved);
                Cell other = w.getLink().getCell();
                if (c.getBiome() == other.getBiome() && c != other && !c.setPiece && !other.setPiece)
                {
                    w.remove(false);
                }
            }
            
        }
    }
}
