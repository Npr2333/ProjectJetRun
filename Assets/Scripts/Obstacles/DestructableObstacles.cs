using System.Collections;
using UnityEngine;

public class DestructableObstacles : MonoBehaviour
{
    public AudioSource speaker;
    public AudioClip deathClip;
    public GameObject deathCross;
    public Animator animator;
    public ParticleSystem deathBoom;
    public PlaneHealth player;
    public int Damage = 20;
    public int health = 100;
    public ParticleSystem DestructionEffect;
    public float destoryTime = 10f;
    public ScoreQueue scoreQueue;
    public int killScore = 150;
    private Vector3 ContactPoint;
    private bool isDead = false;

    void Start()
    {
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        if(gameManager && GameManager.Instance.player != null)
            player = GameManager.Instance.player.GetComponent<PlaneHealth>();
        deathCross.SetActive(false);
        StartCoroutine(Suicide(destoryTime));
    }
    public void OnHit(int damageTaken)
    {
        // When bullet hits this thing
            health -= damageTaken;
            if (health <= 0 && !isDead)
            {
                isDead = true;
                scoreQueue.AddToList(new ScoreObject("BH-94: +" + killScore, 3f, killScore));
                Explode();
                deathCross.SetActive(true);
            }
    }
    private void Explode()
    {
        //Handle the particle system of plane explosion
        ParticleSystem explosion = Instantiate(DestructionEffect, transform.position, Quaternion.identity);
        explosion.transform.parent = gameObject.transform;
        explosion.Play();
        speaker.Stop();
        speaker.clip = deathClip;
        speaker.Play();
        StartCoroutine(startDeath(3f));

    }

    private IEnumerator startDeath(float time)
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(time);
        deathBoom.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void setTarget(GameObject plane)
    {
        player = plane.GetComponent<PlaneHealth>();
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

    IEnumerator Suicide(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

}
