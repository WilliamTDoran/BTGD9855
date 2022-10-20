using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGenerator
{
    public enum Biome
    {
        strgoi,
        yara,
        impundulu,
        length
    }

    Maze m;

    public static Color[] BIOMECOLOR = { new Color(0, 0, 1), new Color(0, 1, 0), new Color(1, 0, 0), new Color(1, 1, 1) };

    private Wall[][] biomesAdding;

    public BiomeGenerator(Maze temp)
    {
        m = temp;
        biomesAdding = new Wall[(int)Biome.length - 1][];
        for (int i = 0; i < biomesAdding.Length; i++)
        {
            biomesAdding[i] = new Wall[0];
        }
    }

    void addToList(int listNum, Wall adding)
    {
        Wall[] temp = new Wall[biomesAdding[listNum].Length + 1];
        for (int i = 0; i < biomesAdding[listNum].Length; i++)
        {
            temp[i] = biomesAdding[listNum][i];
        }
        temp[biomesAdding[listNum].Length] = adding;
        biomesAdding[listNum] = temp;
    }

    void removeFromList(int listNum, int removing)
    {
        Wall[] temp = new Wall[biomesAdding[listNum].Length - 1];
        for (int i = 0; i < temp.Length; i++)
        {
            if (i < removing) 
            {
                temp[i] = biomesAdding[listNum][i];
            } else
            {
                temp[i] = biomesAdding[listNum][i+1];
            }
        }
        biomesAdding[listNum] = temp;
    }


    public void generateBiomes(int centerX, int centerY)
    {
        Cell next = m.getCell(centerX, centerY);
        Cell temp = next.getWall((int)Wall.wLocation.east).getLink().getCell();
        addToList(0, next.getWall((int)Wall.wLocation.east));
        Debug.Log(temp.getBiome());
        temp = next.getWall((int)Wall.wLocation.west).getLink().getCell();
        addToList(1, next.getWall((int)Wall.wLocation.west));
        
        for (int done = 0; done < biomesAdding.Length; done += 0)
        {
            for (int nextBiome = 0; nextBiome < biomesAdding.Length; nextBiome++)
            {
                if (biomesAdding[nextBiome].Length == 0) continue;
                //pick cell from list
                do
                {
                    int cellChosen = Random.Range(0, biomesAdding[nextBiome].Length);
                    next = biomesAdding[nextBiome][cellChosen].getLink().getCell();
                    removeFromList(nextBiome, cellChosen);
                } while (next.getBiome() != Biome.length && biomesAdding[nextBiome].Length > 0);
                if (biomesAdding[nextBiome].Length == 0 && next.getBiome() != Biome.length)
                {
                    done++;
                } else
                {
                    //add biome color
                    next.setBiome((Biome) nextBiome);
                    // add adjacent walls to list
                    for (int i=0; i<4; i++)
                    {
                        addToList(nextBiome, next.getWall(i));
                    }
                }
            }
        }
    }
    
}
