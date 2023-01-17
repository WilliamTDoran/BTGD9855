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


[System.Serializable]
public class MazeVariables
{
    public int width;
    public int height;
    public float scale;
    public Biome CharacterBiome;
}


[System.Serializable]
public class BiomeVariables
{
    public float NextCellChance; // Not implemented
    public float RandomCellChance; // not implemented
    public Color colour;
    public float wallsRemoved; // not implemented
    public int maxCellDifficulty;
    [Tooltip("Array of the objects to be placed in the maze")]
    public PlacableObject[] objects;
}


[System.Serializable]
public class PlacableObject
{
    public GameObject formation;
    public float cellDifficulty; // cells can't weigh more than maxCellDifficulty
    public int maxPlaced;
    public float minPlaced;
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
