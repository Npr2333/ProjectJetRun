using System.Collections;
using UnityEngine;

public class PlaneHealth : MonoBehaviour
{
    public HitSpeaker hitSpeaker;
    public int setHealth = 100;
    public int health;
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
        hitSpeaker.Play();
        if (cameraInstance)
        {
            cameraInstance.ShakeCamera(shakeIntensity, shakeTime);
        }
    }

    //public void OnHit(int damage)
    //{
    //    health -= damage;
    //    hitSpeaker.Play();
    //    if (cameraInstance)
    //    {
    //        cameraInstance.ShakeCamera(shakeIntensity, shakeTime);
    //    }
    //}

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
        StartCoroutine(Explode());
        FindObjectOfType<GameManager>().SetCurrentState(GameManager.GameState.GameOver);
    }

    IEnumerator Explode()
    {
        ParticleSystem explosion = Instantiate(ExplosionEffect, ContactPoint, Quaternion.identity);
        explosion.Play();
        yield return new WaitForSeconds(explosion.main.duration);
        gameObject.SetActive(false);
        //Destroy(explosion);
    }

    //private void Explode()
    //{
    //    //Handle the particle system of plane explosion
    //    ParticleSystem explosion = Instantiate(ExplosionEffect, ContactPoint, Quaternion.identity);
    //    explosion.Play();
    //    Destroy(explosion,1f);
        
    //}

    private void Update()
    {
        if (health <= 0)
        {
            Death();
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        health = setHealth;
    }
    public int getHealth()
    {
        return (int)Mathf.Ceil(100 * (float)health / setHealth);
    }

    public void resetHealth()
    {
        health = setHealth;
    }
}
