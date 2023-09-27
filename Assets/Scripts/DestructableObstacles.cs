using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class DestructableObstacles : MonoBehaviour
{
    public PlaneHealth player;
    public int Damage = 20;
    public int health = 100;
    public ParticleSystem DestructionEffect;
    private Vector3 ContactPoint;

    void Start()
    {

    }
    public void OnHit(int damageTaken)
    {
        // When bullet hits this thing
            health -= damageTaken;
            if (health <= 0)
            {   
                ParticleSystem explode = Instantiate(DestructionEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                //Destroy(explode);
            }
    }

    private void OnCollisionEnter(Collision other) 
    {   
        Debug.Log("Entered");
        if (other.gameObject.tag == "Plane")
        {
            ContactPoint = other.contacts[0].point;
            player.ReduceHealth(Damage, ContactPoint);
        }

        if(other.gameObject.tag == "Bullets")
        {
            Debug.Log("Hit");
            BulletController bc = other.gameObject.GetComponent<BulletController>();
            if(bc != null)
            {
                OnHit(bc.damage);
            }
        }
    }

}
