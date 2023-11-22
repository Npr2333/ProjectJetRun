using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class ProceduralMeshGeneration : MonoBehaviour
{
    public class Line
    {
        public Vector3 Start { get; private set; }
        public Vector3 End { get; private set; }

        public Line(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
        }

        // Method to calculate the closest point on the line segment to a given point
        public Vector3 ClosestPoint(Vector3 point)
        {
            Vector3 lineDirection = End - Start;
            float length = lineDirection.magnitude;
            lineDirection.Normalize();

            float projectLength = Mathf.Clamp(Vector3.Dot(point - Start, lineDirection), 0, length);
            return Start + lineDirection * projectLength;
        }
    }

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    public Transform startTransform;
    public Transform endTransform;
    public int xSize = 20;
    public int zSize = 20;
    public float strength = 0.3f;
    public float height = 2f;
    public int xCoord = 0;
    public int zCoord = 0;
    public float valleyWidth = 1f;
    public float valleyDepth = 1f;
    public float minimumDepth = -10f;

    void CreateShape()
    {
        Vector3 startPoint = startTransform.position;
        Vector3 endPoint = endTransform.position;
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        Vector3 line = endPoint - startPoint;
        float lineLength = line.magnitude;
        line.Normalize();


        //for (int i = 0, z = 0; z <= zSize; z++)
        //{
        //    for (int x = 0; x <= xSize; x++)
        //    {
        //        float y = Mathf.PerlinNoise((x + xCoord) * strength, (z + zCoord) * strength) * height;
        //        vertices[i] = new Vector3(x, y, z);
        //        i++;
        //    }
        //}

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise((x + xCoord) * strength, (z + zCoord) * strength) * height;

                // Calculate the closest point on the line to the vertex
                Line valley = new Line(startPoint, endPoint);
                Vector3 vertexPosition = transform.position + new Vector3(x * transform.localScale.x, y * transform.localScale.y, z * transform.localScale.z);
                Debug.Log("Veretx posiiton is: " + vertexPosition);
                Vector3 closestPoint = valley.ClosestPoint(vertexPosition);
                float distanceToLine = (vertexPosition - closestPoint).magnitude;
                if (distanceToLine < valleyWidth)
                {
                    y -= (1 - (distanceToLine/(valleyWidth)) * (distanceToLine /(valleyWidth))) * valleyDepth;
                }
                y = Mathf.Max(-minimumDepth, y);
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

            triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    public void generateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }
}

