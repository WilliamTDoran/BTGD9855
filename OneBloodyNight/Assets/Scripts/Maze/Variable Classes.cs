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
    public GameObject[] hubObjects;
    public Biome CharacterBiome;
    public GameObject bossPortal;
}

[System.Serializable]
public class spawnerVars
{
    [Tooltip("min_spawn_attempts")]
    public int minAttempts;
    [Tooltip("max_spawn_attempts")]
    public int maxAttempts;
    [Tooltip("max_enemies")]
    public int maxCount;
    [Tooltip("base_chance")]
    public float baseChance = 0.7f;
    [Tooltip("spawn_steepness")]
    public float steepness = 1.7f;
}


[System.Serializable]
public class BiomeVariables
{
    public Color colour;
    public float wallsRemoved;
    public int maxCellDifficulty;
    [Tooltip("Coming Soon!")]
    public GameObject[] SetPieces;
    [Tooltip("Array of the objects to be placed in the maze")]
    public PlacableObject[] objects;
    public GameObject[] enemies;
    public ArtVars North;
    public ArtVars East;
    [Tooltip("Use the following order: All, missingNorth, missingEast, missingSouth, missingWest, lineVertical, lineHorizontal, cornerNorthWest, cornerNorthEast, cornerSouthEast, cornerSouthWest, endNorth, endEast, endSouth, endWest, None")]
    public ArtVars Corner;
    public ArtVars floors;
}

[System.Serializable]
public class EnemySpawnVars
{

}

public class ObjectSpawnVars
{

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
