using System.Collections;
using UnityEngine;

public class AudioSyncDisplacement : AudioSyncer
{
    public MeshFilter meshFilter;
    public float beatDisplacement = 1f; // Displacement amount on beat
    //public float restDisplacement = 0f;
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;
    private float currentDisplacement = 0;

    private void Start()
    {
        // Ensure there's a mesh filter component attached
        if (!meshFilter) meshFilter = GetComponent<MeshFilter>();

        mesh = meshFilter.mesh;
        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        System.Array.Copy(originalVertices, displacedVertices, originalVertices.Length);
    }

    private IEnumerator MoveToDisplace(float displacement)
    {
        float initial = currentDisplacement;
        float timer = 0;
        Debug.Log("Here");
        while (currentDisplacement != displacement)
        {
            currentDisplacement = Mathf.Lerp(initial, displacement, timer / timeToBeat);
            timer += Time.deltaTime;

            DisplaceVertices(currentDisplacement);
            mesh.vertices = displacedVertices;
            mesh.RecalculateBounds();

            yield return null;
        }

        m_isBeat = false;
    }
    public override void OnBeat()
    {
        base.OnBeat();
        // Displace vertices on beat
        //DisplaceVertices(beatDisplacement);
        // Apply the displaced vertices to the mesh
        //mesh.vertices = displacedVertices;
        //mesh.RecalculateBounds();

        StopCoroutine("MoveToDisplace");
        StartCoroutine("MoveToDisplace", beatDisplacement);
    }

    private void DisplaceVertices(float displacementAmount)
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            // Displace the vertex in the direction of its normal
            displacedVertices[i] = originalVertices[i] + (mesh.normals[i] * displacementAmount);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_isBeat) return;
        // If not currently in a beat, lerp the vertices back to their original position
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            displacedVertices[i] = Vector3.Lerp(displacedVertices[i], originalVertices[i], restSmoothTime * Time.deltaTime);
        }

        // Apply the updated vertices to the mesh
        mesh.vertices = displacedVertices;
        mesh.RecalculateBounds();
    }
}
