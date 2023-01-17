using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BiomeGenerator
{
    private Wall[][] biomesAdding;

    public BiomeGenerator()
    {
        biomesAdding = new Wall[(int)Biome.length][];
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

        //set up biome generator
        Cell next = Maze.m.getCell(centerX, centerY);
        int one = (((int)next.getBiome()) + 1) % 3;
        Debug.Log(one);
        int two = (((int)next.getBiome()) + 2) % 3;
        Debug.Log(two);
        addToList(one, next.getWall((int)Wall.wLocation.east));
        addToList(two, next.getWall((int)Wall.wLocation.west));
        
        //generating biomes
        for (int done = 1; done < biomesAdding.Length; done += 0)
        {
            //Skip biomes with nothing more to add
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
