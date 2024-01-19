using System;
using System.Collections;
using UnityEngine;

public class IndustructableObstacles : MonoBehaviour
{
    public GameObject model;
    public AudioSource speaker;
    public AudioClip deathClip;
    public ParticleSystem DestructionEffect;
    public ParticleSystem deathBoom;
    public Animator animator;
    [HideInInspector]public CharacterController controller;
    public PlaneHealth player;
    public int Damage = 100;
    public int health = 100;
    public float destoryTime = 10f;
    public GameObject deathCross;
    public bool noDestroy = false;
    public ScoreQueue scoreQueue;
    public int killScore = 300;
    private Vector3 ContactPoint;
    private Vector3 direction;
    private bool isDead = false;
    private void Start()
    {
        //controller = GetComponent<CharacterController>();
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        if (gameManager && GameManager.Instance.player != null)
            player = GameManager.Instance.player.GetComponent<PlaneHealth>();

        if (!noDestroy)
        {
            StartCoroutine(Suicide(destoryTime));
        }
    }

    private void FixedUpdate()
    {
        if (direction != null)
        {
            transform.position += direction * Time.deltaTime;
        }
    }

    public  void OnHit(int damageTaken)
    {
        health -= damageTaken;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            scoreQueue.AddToList(new ScoreObject("RG-7: +" + killScore, 3f, killScore));
            Explode();
            deathCross.SetActive(true);
        }
    }

    public void setTarget(GameObject plane)
    {
        player = plane.GetComponent<PlaneHealth>();
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Plane")
        {
            ContactPoint = other.contacts[0].point;
            player.ReduceHealth(Damage, ContactPoint);
        }    
    }

    public Vector3 GetContactPosition()
    {   
        if(ContactPoint != null)
        {
            return ContactPoint;
        }
        throw new Exception("No contacts yet");
    }

    IEnumerator Suicide(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    public void setMovement(Vector3 heading)
    {
        direction = heading;
    }

    private void Explode()
    {
        //Handle the particle system of plane explosion
        ParticleSystem explosion = Instantiate(DestructionEffect, transform.position, Quaternion.identity);
        explosion.transform.parent = gameObject.transform;
        explosion.Play();
        //speaker.Stop();
        //speaker.clip = deathClip;
        //speaker.Play();
        model.SetActive(false);
        StartCoroutine(startDeath(explosion.duration));

    }

    private IEnumerator startDeath(float time)
    {
        //animator.SetBool("isDead", true);
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
