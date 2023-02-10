using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;


public enum Biome
{
    strgoi,
    yara,
    impundulu,
    length
}


internal enum CornerType
{
    All, //48
    missingNorth,
    missingEast, //50
    missingSouth,
    missingWest, //49
    lineVertical, //a few
    lineHorizontal, 
    cornerNorthWest,
    cornerNorthEast,
    cornerSouthEast, // 43
    cornerSouthWest, // 42
    endNorth,
    endEast, //47
    endSouth,
    endWest, //46
    None
}


[System.Serializable]
public class MazeVariables
{
    public int width;
    public int height;
    public float scale;
    public float betweenCells;
    public Biome CharacterBiome;
}


[System.Serializable]
public class BiomeVariables
{
    //public float NextCellChance; // Not implemented
    //public float RandomCellChance; // not implemented
    public Color colour;
    public float wallsRemoved;
    public int maxCellDifficulty;
    [Tooltip("Array of the objects to be placed in the maze")]
    public PlacableObject[] objects;
    public ArtVars North;
    public ArtVars East;
    [Tooltip("Use the following order: All, missingNorth, missingEast, missingSouth, missingWest, lineVertical, lineHorizontal, cornerNorthWest, cornerNorthEast, cornerSouthEast, cornerSouthWest, endNorth, endEast, endSouth, endWest, None")]
    public ArtVars Corner;
}


[System.Serializable]
public class PlacableObject
{
    public GameObject formation;
    public float cellDifficulty; // cells can't weigh more than maxCellDifficulty
    public int maxPlaced;
    public float minPlaced;
}


[System.Serializable]
public class ArtVars
{
    public Vector3 offSet;
    public GameObject[] Sprites;
}

public class Spawn
{
    internal int x;
    internal int y;
    internal GameObject obj;
    internal Spawn(int x, int y, GameObject obj)
    {
        this.x = x;
        this.y = y;
        this.obj = obj;
    }
}
