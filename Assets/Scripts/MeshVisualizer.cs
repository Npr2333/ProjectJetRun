using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshVisualizer : MonoBehaviour
{
    private Mesh mesh;
    public Color lineColor = Color.gray;
    void OnDrawGizmos()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        if (mesh == null)
        {
            return;
        }

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        Gizmos.color = Color.red;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = transform.TransformPoint(vertices[triangles[i]]);
            Vector3 p2 = transform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 p3 = transform.TransformPoint(vertices[triangles[i + 2]]);

            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p1);
            Gizmos.color = lineColor;
        }
    }
}
