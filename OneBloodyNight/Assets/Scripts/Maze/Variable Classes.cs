using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

/// <summary>
/// Enumerated Type for the different biomes in the game.
/// </summary>
public enum Biome
{
    strgoi,
    yara,
    impundulu,
    length
}


/// <summary>
/// Enumerated type for the order in which the corner sprites are to be put in
/// </summary>
internal enum CornerType
{
    All,
    missingNorth,
    missingEast,
    missingSouth,
    missingWest, 
    lineVertical, 
    lineHorizontal, 
    cornerNorthWest,
    cornerNorthEast,
    cornerSouthEast, 
    cornerSouthWest, 
    endNorth,
    endEast, 
    endSouth,
    endWest, 
    None
}


/// <summary>
/// The variables that affect the maze as a whole
/// </summary>
[System.Serializable]
public class MazeVariables
{
    [Header("Maze Sizing")]
    [Tooltip("How many cells wide the maze is.")]
    public int width;
    [Tooltip("How many cells high the maze is.")]
    public int height;
    [Tooltip("General Scale factor to the entire maze. Everything that becomes a child of the maze at runtime is scaled by this much. (Essentially, everything spawned in)")]
    public float scale;
    [Tooltip("The biome of the hub")]
    public Biome CharacterBiome;

    [Header("Maze Spawns Variables")]
    [Tooltip("Minimum number of setpieces that spawn in")]
    public int minLocation = 1;
    [Tooltip("Chance of spawning a pickup in any one square")]
    public float chanceObj;
    [Tooltip("Offset to align cells correctly")]
    public float betweenCells;

    [Header("Maze Spawn Lists: All Biomes")]
    [Tooltip("Anything that spawns in the hub goes here. Have it's x & z be equal to it's offset of where in the hub you want it.")]
    public GameObject[] hubObjects;
    [Tooltip("List of everything that spawns in the exact center and has a limit of 1 per cell. Examples include upgrade totem, lore books, and so on")]
    public GameObject[] pickupsObjects;
}


/// <summary>
/// The variables for the biomes individually
/// </summary>
[System.Serializable]
public class BiomeVariables
{
    [Header("Variables for this Biome")]
    [Tooltip("Colour isn't used. Mostly useful for telling what biome is what biome in the inspector quickly")]
    public Color colour;
    [Tooltip("How many walls are removed from the maze after generation to make it flow better")]
    public float wallsRemoved;
    [Tooltip("Chance of spawning an object. (1 = 100%)")]
    public float chanceObject;

    [Header("Object lists!")]
    [Tooltip("The portal to teleport you to the bossroom")]
    public GameObject bossPortal;
    [Tooltip("List of the setpieces. IMPORTANT: Setpieces need to be made a certain way. Please only make one if you know how to make one.")]
    public GameObject[] SetPieces;
    [Tooltip("Array of the decore to be placed in this biome. Spawns at most 4 per biome, 1 per corner kinda thing.")]
    public GameObject[] objects;
    [Tooltip("Array of the enemies to be placed in this biome. Can be either singular enemies, or formations of enemies")]
    public GameObject[] enemies;

    [Header("Lists of Art!")]
    [Tooltip("Array of the different northern wall sprites. (Also used as southern walls)")]
    public ArtVars North;
    [Tooltip("Array of the different northern eastern sprites. (Also used as western walls)")]
    public ArtVars East;
    [Tooltip("Use the following order: All, missingNorth, missingEast, missingSouth, missingWest, lineVertical, lineHorizontal, cornerNorthWest, cornerNorthEast, cornerSouthEast, cornerSouthWest, endNorth, endEast, endSouth, endWest, None")]
    public ArtVars Corner;
    [Tooltip("Array of the different floors for the biome.")]
    public ArtVars floors;
}


/// <summary>
/// the variables for the different art arrays. Used in Biome Variables only
/// </summary>
[System.Serializable]
public class ArtVars
{
    [Tooltip("How far the sprite needs to be offset to line up nicely")]
    public Vector3 offSet;
    [Tooltip("List of the possible sprites to spawn in")]
    public GameObject[] Sprites;
}


/// <summary>
/// Variables for the spawners
/// </summary>
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