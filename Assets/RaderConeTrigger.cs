using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaderConeTrigger : MonoBehaviour
{
    private List<GameObject> objectsInTrigger = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DestructableObstacles" || other.gameObject.tag == "Boss")
        {
            //Debug.Log("Added");
            if (!objectsInTrigger.Contains(other.gameObject))
            {
                objectsInTrigger.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInTrigger.Contains(other.gameObject))
        {
            objectsInTrigger.Remove(other.gameObject);
        }
    }

    public List<GameObject> GetObjectsInTrigger()
    {
        objectsInTrigger.RemoveAll(item => item == null);
        return objectsInTrigger;
    }
}
