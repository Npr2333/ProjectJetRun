using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralMeshGeneration))]
public class MeshGenerationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the default inspector

        ProceduralMeshGeneration generate = (ProceduralMeshGeneration)target;
        if (GUILayout.Button("Generate"))
        {
            generate.generateMesh() ;
        }
    }
}
