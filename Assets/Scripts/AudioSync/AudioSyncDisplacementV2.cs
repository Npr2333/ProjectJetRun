using System.Collections;
using UnityEngine;

public class AudioSyncDisplacementV2 : AudioSyncer
{
    public MeshFilter meshFilter;
    public float beatDisplacement = 1f;
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;
    private Coroutine displacementCoroutine;
    private float currentDisplacement = 0;

    private void Start()
    {
        if (!meshFilter) meshFilter = GetComponent<MeshFilter>();

        // Duplicate the mesh for modification
        mesh = Instantiate(meshFilter.mesh);
        meshFilter.mesh = mesh; // Assign the duplicate mesh

        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        System.Array.Copy(originalVertices, displacedVertices, originalVertices.Length);
    }

    private IEnumerator MoveToDisplace(float targetDisplacement)
    {
        float timer = 0;
        float startDisplacement = currentDisplacement;

        while (timer < timeToBeat)
        {
            currentDisplacement = Mathf.Lerp(startDisplacement, targetDisplacement, timer / timeToBeat);
            DisplaceVertices(currentDisplacement);
            mesh.vertices = displacedVertices;
            mesh.RecalculateNormals(); // Recalculate normals if necessary
            mesh.RecalculateBounds();

            timer += Time.deltaTime;
            yield return null;
        }

        currentDisplacement = targetDisplacement; // Ensure final displacement is set
        m_isBeat = false;
    }

    public override void OnBeat()
    {
        base.OnBeat();
        if (displacementCoroutine != null) StopCoroutine(displacementCoroutine);
        displacementCoroutine = StartCoroutine(MoveToDisplace(beatDisplacement));
    }

    private void DisplaceVertices(float displacementAmount)
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i] + (mesh.normals[i] * displacementAmount);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_isBeat) return;

        for (int i = 0; i < displacedVertices.Length; i++)
        {
            displacedVertices[i] = Vector3.Lerp(displacedVertices[i], originalVertices[i], restSmoothTime * Time.deltaTime);
        }

        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals(); // Recalculate normals if necessary
        mesh.RecalculateBounds();
    }
}
