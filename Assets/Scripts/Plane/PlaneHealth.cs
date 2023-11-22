using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneHealth : MonoBehaviour
{
    public int health = 100;
    public ParticleSystem ExplosionEffect;
    private Vector3 ContactPoint;
    public CameraShake cameraInstance;
    public float shakeIntensity = 10f;
    public float shakeTime = 0.2f;
    public void ReduceHealth(int damage, Vector3 ContactPosition)
    {
        //Handle player takes damage
        ContactPoint = ContactPosition;
        health -= damage;
        if (cameraInstance)
        {
            cameraInstance.ShakeCamera(shakeIntensity, shakeTime);
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
        if (cameraInstance)
        {
            cameraInstance.ShakeCamera(shakeIntensity, shakeTime);
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
        FindObjectOfType<GameManager>().SetCurrentState(GameManager.GameState.GameOver);
    }

    private void Explode()
    {
        //Handle the particle system of plane explosion
        ParticleSystem explosion = Instantiate(ExplosionEffect, ContactPoint, Quaternion.identity);
        explosion.Play();
        Destroy(explosion,1f);
        
    }

    private void Update()
    {
        if (health <= 0)
        {
            Death();
            Destroy(gameObject);
        }
    }
}
