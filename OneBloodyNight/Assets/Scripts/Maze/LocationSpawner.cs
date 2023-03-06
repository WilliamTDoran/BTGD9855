using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationSpawner
{
    /// <summary>
    /// Picks a location for the setpieces
    /// </summary>
    /// <param name="b"></param>
    /// <param name="attempts"></param>
    internal static void placeLocation(Biome b, int attempts)
    {
        Cell[][] setPiece;
        // attempt to place one 3 times
        for (int a=0; a<attempts; a++)
        {
            // pick a random formation
            int val = Random.Range(0, Maze.m.biomeVariables[(int)b].SetPieces.Length);
            GameObject setPieceGO = Maze.m.biomeVariables[(int)b].SetPieces[val];
            setPiece = new Cell[setPieceGO.transform.childCount - 1][];

            // Decypher formation
            /**
             * Structure?
             * GOContainer
             * - GOFeaturedObjects
             * - GOCellsR1
             * - - GOCell1
             * - - GOCell2
             * - GOCellsR2
             * - ...
             * - GOCellsRLast
             * 
             * How cells connect?
             * "Buffer" cells for empty location;
             * GO without the cell script
             * 
             * 
             * C C   C
             *   C C C   C
             *       C C C
             * 
             * Container
             * - Objects
             * - Row1
             * - - Cell1
             * - - Cell2
             * - - BlankCell3
             * - - Cell4
             * - Row2
             * - - BlankCell1
             * - - Cell2
             * - - Cell3
             * - - Cell4
             * - - BlankCell5
             * - - Cell6
             * - Row2
             * - - BlankCell1
             * - - BlankCell2
             * - - BlankCell3
             * - - Cell4
             * - - Cell5
             * - - Cell6
             * 
             * */
            //Debug.Log("Count of children in setPieceGO: "+setPieceGO.transform.childCount);
            for (int i=1; i< setPieceGO.transform.childCount; i++)
            {
                //Debug.Log("On loop number:"+i+", "+ setPieceGO.transform.GetChild(i).childCount+", Length of ");
                setPiece[i-1] = new Cell[setPieceGO.transform.GetChild(i).childCount];
                //Debug.Log("Length in setPiece["+i+"]: " + setPiece[i].Length);
                for (int j=0; j< setPieceGO.transform.GetChild(i).childCount; j++)
                {
                    if (setPieceGO.transform.GetChild(i).GetChild(j).GetComponent<Cell>() == null)
                    {
                        setPiece[i-1][j] = null;
                        continue;
                    }
                    setPiece[i-1][j] = setPieceGO.transform.GetChild(i).GetChild(j).GetComponent<Cell>();
                    //setPiece[i-1][j].setPiece = true;
                    //setPiece[i][j] is now the Cell component of the cells there
                }
            }

            // pick a location in the biome for the formation
            Vector2 setPieceLocation = new Vector2(Random.Range(0, Maze.m.width()), Random.Range(0, Maze.m.height()));
            Debug.Log("Set piece attempted to place at location " + setPieceLocation.x + ", " + setPieceLocation.y);
            bool placeable = true;
            // check if the formation fits within the confines of the maze & biome
            for (int i=0; i<setPiece.Length && placeable; i++)
            {
                for (int j=0; j<setPiece[i].Length; j++)
                {
                    /*
                     * (setPiece[i][j] != null && 
                     * (int)setPieceLocation.x + i < Maze.m.width() && 
                     * (int)setPieceLocation.y + j < Maze.m.height() && 
                     * Maze.m.getCell((int)setPieceLocation.x + i, (int)setPieceLocation.y + j).getBiome() != b && 
                     * !Maze.m.getCell((int)setPieceLocation.x + i, (int)setPieceLocation.y + j).setPiece) || 
                     * (setPiece[i][j] != null && 
                     * ((int)setPieceLocation.x + i > Maze.m.width() || 
                     * (int)setPieceLocation.y + j > Maze.m.height()))
                     */
                    if (setPiece[i][j] != null) // if cell ij is in setpiece
                    {
                        if ((int)setPieceLocation.x + i < Maze.m.width() && (int)setPieceLocation.y + j < Maze.m.height()) //if cell in setpiece is in maze
                        {
                            if (Maze.m.getCell((int)setPieceLocation.x + i, (int)setPieceLocation.y + j).getBiome() != b) // if cell biome doesn't match
                            {
                                placeable = false;
                                break;
                            } else if (Maze.m.getCell((int)setPieceLocation.x + i, (int)setPieceLocation.y + j).setPiece) // if determined location for setpiece is already a setpiece
                            {
                                placeable = false;
                                break;
                            }
                        } else //cell in setpiece isn't in maze
                        {
                            placeable = false;
                            break;
                        }
                        
                    }
                }
            }
            Debug.Log("Test");
            if (!placeable)
            {
                Debug.Log("No set piece!");
                continue;
            } else
            {
                Debug.Log("Set piece able to be placed!");
            }

            // if yes, place it
            for (int i = 0; i < setPiece.Length && placeable; i++)
            {
                for (int j = 0; j < setPiece[i].Length; j++)
                {
                    if (setPiece[i][j] == null)
                    {
                        Debug.Log("Maze cell at " + ((int)setPieceLocation.x + i) + ", " + ((int)setPieceLocation.y + j) + " not included in set piece");
                        continue;
                    }
                    // Set walls in maze equal to walls in cell, only inside the formation
                    /*if (j - 1 >= 0 && setPiece[i][j - 1] != null && !setPiece[i][j].getWall((int)Wall.wLocation.north).gameObject.activeSelf) 
                    {
                        //Maze.m.getCell((int)setPieceLocation.x + i, (int)setPieceLocation.y + j).getWall((int)Wall.wLocation.north).remove(false);

                    }
                    if (i + 1 < setPiece[i].Length && setPiece[i + 1][j] != null && !setPiece[i][j].getWall((int)Wall.wLocation.east).gameObject.activeSelf) 
                    {
                        //Maze.m.getCell((int)setPieceLocation.x + i, (int)setPieceLocation.y + j).getWall((int)Wall.wLocation.east).remove(false);
                    }*/
                    Debug.Log("Maze cell at "+ ((int)setPieceLocation.x + i)+", "+((int)setPieceLocation.y + j)+" placed in set piece");
                    Maze.m.getCell((int)setPieceLocation.x + i, (int)setPieceLocation.y + j).setPiece = true;
                    Maze.m.getCell((int)setPieceLocation.x + i, (int)setPieceLocation.y + j).putInMaze();

                }
            }
        }
    }
}
