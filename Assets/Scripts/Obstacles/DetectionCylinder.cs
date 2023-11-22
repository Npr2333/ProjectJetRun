using System.Collections;
using System.Collections.Generic;
using Tarodev;
using UnityEngine;

public class DetectionCylinder : MonoBehaviour
{
    public int damage = 100;

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "FX" && collision.gameObject.tag != "Bullets" && collision.gameObject.tag != "DestructableObstacles")
        {
            Rigidbody hitRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize();

            if (collision.gameObject.tag == "Plane")
            {
                PlaneHealth health = collision.gameObject.GetComponent<PlaneHealth>();
                health.ReduceHealth(damage, gameObject.transform.position);
            }
        }
    }
}
