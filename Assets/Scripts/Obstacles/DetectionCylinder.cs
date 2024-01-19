using System.Collections;
using System.Collections.Generic;
using Tarodev;
using UnityEngine;

public class DetectionCylinder : MonoBehaviour
{
    public int damage = 100;
    public bool isFriencly = false;
    public void OnTriggerEnter(Collider collision)
    {
        if (!isFriencly)
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
        else
        {
            if (collision.gameObject.tag == "DestructableObstacles")
            {
                if (collision.gameObject.tag == "DestructableObstacles")
                {
                    DestructableObstacles obstacle = collision.gameObject.GetComponent<DestructableObstacles>();
                    obstacle.OnHit(damage); 
                }
                else if (collision.gameObject.tag == "IndestructableObstacles")
                {
                    IndustructableObstacles obstacle = collision.gameObject.GetComponent<IndustructableObstacles>();
                    obstacle.OnHit(damage);
                }
            }
        }
    }

    public void setDamage(int num)
    {
        damage = num;
    } 
}
