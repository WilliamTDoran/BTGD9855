using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationSpawner
{
    /// <summary>
    /// Picks a location for the 
    /// </summary>
    /// <param name="b"></param>
    /// <param name="attempts"></param>
    internal static void placeLocation(Biome b, int attempts)
    {
        // attempt to place one 3 times
        for (int i=0; i<attempts; i++)
        {
            // pick a random formation
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

            // pick a location in the biome for the formation
            // check if the formation fits within the confines of the maze & biome
            // if no, continue
            // if yes, place it
        }
    }
}
