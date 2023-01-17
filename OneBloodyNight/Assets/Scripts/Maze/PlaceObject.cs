using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject
{
    public void massPlace(Biome b, PlacableObject obj)
    {
        for (int i = Random.Range(obj.minPlaced, obj.maxPlaced); i > 0; i--) 
        {
            place(b, obj);
        }
    }

    public void place(Biome b, PlacableObject obj)
    {
        int x;
        int y;
        cell(b, obj.maxPerCell, obj.cellDifficulty, out x, out y);
        //GameObject placed = GameObject.Instantiate(obj.placing, new Vector3(Maze.m.getWorldPosX(x), Maze.m.getWorldPosX(x), obj.placing.transform.position.z), new Quaternion());
        Cell c = Maze.m.getCell(x, y);
        //placed.transform.parent = c.spawner.transform;
    }

    public void cell(Biome b, short maxPerCell, float diff, out int x, out int y)
    {
        Cell c;
        do
        {
            x = Random.Range(0, Maze.m.width());
            y = Random.Range(0, Maze.m.height());
            c = Maze.m.getCell(x, y);
            // while the biomes don't match & the cell's difficulty is too high & the
        } while (c.getBiome() != b && diff+c.difficulty >= Maze.m.biomeVariables[(int)c.getBiome()].maxCellDifficulty && c.placed > maxPerCell);
    }
}
