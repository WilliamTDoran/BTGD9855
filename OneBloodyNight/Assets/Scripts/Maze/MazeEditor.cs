using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/*
[CustomEditor(typeof(Maze)), CanEditMultipleObjects]
public class MazeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Maze"))
        {
            if (Maze.m == null)
            {
                Maze.m = FindObjectOfType<Maze>();
                Maze.m.RegenMaze();
            } else
            {
                Maze.m.RegenMaze();
            }
        }
        if (GUILayout.Button("Clear Maze"))
        {
            if (Maze.m != null)
            {
                while (Maze.m.transform.childCount > 0)
                {
                    DestroyImmediate(Maze.m.transform.GetChild(0).gameObject);
                }
                if (!Application.isPlaying)
                {
                    Maze.m.clearVars();
                    Maze.m = null;
                }
            }
        }
        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}*/
