using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float pushForce = 10.0f;
    public int damage = 10;
    public ParticleSystem explode;

    void Start()
    {
        
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Rigidbody hitRigidbody = collision.gameObject.GetComponent<Rigidbody>();

    //    if (hitRigidbody != null)
    //    {
    //        Vector3 direction = collision.transform.position - transform.position;
    //        direction.Normalize();

    //        hitRigidbody.AddForce(direction * pushForce, ForceMode.Impulse);
    //    }
    //}
       
    private void OnTriggerEnter(Collider collision)
    {
        DestructableObstacles obstacle = collision.gameObject.GetComponent<DestructableObstacles>();
            if (obstacle != null)
            {
                obstacle.OnHit(damage);
            }

    }
}

