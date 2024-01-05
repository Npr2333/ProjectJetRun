using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Plane")
        {
            PlaneHealth planeHealth = collision.gameObject.GetComponent<PlaneHealth>();
            planeHealth.ReduceHealth(1000, collision.gameObject.transform.position);
        }
    }
}
