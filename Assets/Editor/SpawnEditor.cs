using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnController))]
public class SpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the default inspector

        SpawnController spawn = (SpawnController)target;
        if (GUILayout.Button("Start Spawn"))
        {
            spawn.startSpawn(); ;
        }
    }
}