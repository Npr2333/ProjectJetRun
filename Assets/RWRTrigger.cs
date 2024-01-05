using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RWRTrigger : MonoBehaviour
{
    private List<GameObject> objectsInTrigger = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "DestructableObstacles" || other.gameObject.tag == "IndestructableObstacles" || other.gameObject.tag == "Boss" || other.gameObject.tag == "EnemyMissile" || other.gameObject.tag == "SplineBoss")
        //{
        //    //Debug.Log("Added");
        //    if (!objectsInTrigger.Contains(other.gameObject))
        //    {
        //        objectsInTrigger.Add(other.gameObject);
        //    }
        //}

        if (other.gameObject.tag == "EnemyMissile")
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
    private void Update()
    {
        objectsInTrigger.RemoveAll(item => item == null);
    }
}
