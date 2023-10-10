using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneHealth : MonoBehaviour
{
    public int health = 100;
    public ParticleSystem ExplosionEffect;
    private Vector3 ContactPoint;
    public void ReduceHealth(int damage, Vector3 ContactPosition)
    {
        //Handle player takes damage
        ContactPoint = ContactPosition;
        health -= damage;
        if(health <= 0)
        {
            Death();
            Destroy(gameObject);
        }
    }

    public void OnHit(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Death();
            Destroy(gameObject);
        }
    }

    public void IncreaseHealth(int amount)
    {
        //Handle player takes heal
        health = Mathf.Min(health + amount, 100);
    }

    public int GetHealth()
    {
        //Return player's health
        return health;
    }

    private void Death()
    {
        //Handle player Death
        Debug.Log("Player is dead");
        Explode();
    }

    private void Explode()
    {
        //Handle the particle system of plane explosion
        ParticleSystem explosion = Instantiate(ExplosionEffect, ContactPoint, Quaternion.identity);
        explosion.Play();
        Destroy(explosion,1f);
        
    }
}
