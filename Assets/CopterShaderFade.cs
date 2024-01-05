using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RootMotion.FinalIK.RagdollUtility;

public class CopterShaderFade : MonoBehaviour
{
    public float effectTime = 2f;
    public Material targetMaterial; // Assign this in the inspector or through code
    public List<GameObject> childrens = new List<GameObject>();


    public void FadeIn()
    {
        StartCoroutine(In());
    }
    IEnumerator In()
    {
        float timer = 0;

        float initial = childrens[0].GetComponent<Renderer>().material.GetColor("_Color").a;

        while (timer < effectTime)
        {
            float lerpNum = Mathf.Lerp(initial, 1, timer / effectTime);

            Color temp1 = childrens[0].GetComponent<Renderer>().material.GetColor("_Color");
            Color temp2 = childrens[0].GetComponent<Renderer>().material.GetColor("_Wireframe_Color");
            temp1.a = lerpNum;
            temp2.a = lerpNum;
            foreach (GameObject child in childrens)
            {
                child.GetComponent<Renderer>().material.SetColor("_Color", temp1);
                child.GetComponent<Renderer>().material.SetColor("_Wireframe_Color", temp2);
            }
            timer += Time.deltaTime;
            yield return null;

        }

    }

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
