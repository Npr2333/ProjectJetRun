using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizeController : MonoBehaviour
{
    public Transform central; // Reference to the player's transform
    public Transform valley;
    public Transform cave;
    private List<GameObject> valleyObjects = new List<GameObject>(); // List of objects to check distance
    private List<GameObject> caveObjects = new List<GameObject>();
    public float deactivationDistance = 50.0f; // Distance threshold for deactivation
    public float checkInterval = 1.0f; // How often to check the distances (in seconds)
    public bool started = false;
    private void Start()
    {
        foreach (Transform child in valley)
        {
            valleyObjects.Add(child.gameObject);
        }

        foreach (Transform child in cave)
        {
            caveObjects.Add(child.gameObject);
        }
        // Start the distance check coroutine
        StartCoroutine(CheckDistancesPeriodically());
    }

    private IEnumerator CheckDistancesPeriodically()
    {
        while (true)
        {
            if (started)
            {
                foreach (GameObject obj in valleyObjects)
                {
                    if (obj != null && Vector3.Distance(central.position, obj.transform.position) > deactivationDistance)
                    {
                        obj.SetActive(false); // Deactivate the object if it's beyond the specified distance
                    }
                    else if (obj != null && !obj.activeSelf && Vector3.Distance(central.position, obj.transform.position) <= deactivationDistance)
                    {
                        obj.SetActive(true); // Reactivate the object if it's within the specified distance
                    }
                }
            }

            yield return new WaitForSeconds(checkInterval);
        } 
    }

    public void setOptimize(bool status)
    {
        started = status;
    }
}

