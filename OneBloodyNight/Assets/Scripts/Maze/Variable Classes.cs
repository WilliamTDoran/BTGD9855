using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class PlacableObject
{
    public placeFormation[] formation; // not implemented // todo later: object pool this somehow. discuss ideas with Will
    [Tooltip("Minimum amount of objects that should be placed")]
    public int minPlaced; // not implemented
    [Tooltip("Maximum amount of objects that should be placed")]
    public int maxPlaced; // not implemented
    public float cellDifficulty; // cells can't weigh more than maxCellWeight
    public short maxPerCell;
    [HideInInspector]
    internal int placed;
}

[System.Serializable]
public class placeFormation
{
    public placedObject[] objects;
}

[System.Serializable]
public class placedObject
{
    public GameObject obj;
    public float x;
    public float y;
}
