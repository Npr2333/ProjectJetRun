using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindChildWithShader : MonoBehaviour
{
    public Material targetMaterial; // Assign this in the inspector or through code
    public List<GameObject> childrens = new List<GameObject>();
    public List<GameObject> FindObjectsWithMaterial(GameObject parent)
    {
        List<GameObject> objectsWithMaterial = new List<GameObject>();
        CheckChildren(parent.transform, objectsWithMaterial);
        return objectsWithMaterial;
    }

    private void CheckChildren(Transform parentTransform, List<GameObject> list)
    {
        foreach (Transform child in parentTransform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                foreach (Material mat in renderer.sharedMaterials)
                {
                    if (mat == targetMaterial)
                    {
                        list.Add(child.gameObject);
                        break;
                    }
                }
            }
            CheckChildren(child, list); // Recursively check sub-children
        }
    }

    public void Find()
    {
        childrens = FindObjectsWithMaterial(gameObject);
    }
}
