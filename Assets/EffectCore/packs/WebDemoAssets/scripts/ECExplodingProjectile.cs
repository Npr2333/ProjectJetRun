﻿using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

/* THIS CODE IS JUST FOR PREVIEW AND TESTING */
// Feel free to use any code and picking on it, I cannot guaratnee it will fit into your project
public class ECExplodingProjectile : MonoBehaviour
{
    public GameObject impactPrefab;
    public GameObject explosionPrefab;
    public float thrust;
    public float delay = 3f;

    public float pushForce;
    public int damage;

    public Rigidbody thisRigidbody;

    public GameObject particleKillGroup;
    private Collider thisCollider;

    public bool LookRotation = true;
    public bool Missile = false;
    public Transform missileTarget;
    public float projectileSpeed;
    public float projectileSpeedMultiplier;

    public bool ignorePrevRotation = false;

    public bool explodeOnTimer = false;
    public float explosionTimer;
    float timer;

    private Vector3 previousPosition;

    // Use this for initialization
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        if (Missile)
        {
            missileTarget = GameObject.FindWithTag("Target").transform;
        }
        thisCollider = GetComponent<Collider>();
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*     if(Input.GetButtonUp("Fire2"))
             {
                 Explode();
             }*/
        StartCoroutine(Suicide(delay));
        timer += Time.deltaTime;
        if (timer >= explosionTimer && explodeOnTimer == true)
        {
            Explode();
        }

    }

    void FixedUpdate()
    {
        if (Missile)
        {
            projectileSpeed += projectileSpeed * projectileSpeedMultiplier;
            //   transform.position = Vector3.MoveTowards(transform.position, missileTarget.transform.position, 0);

            transform.LookAt(missileTarget);

            thisRigidbody.AddForce(transform.forward * projectileSpeed);
        }

        if (LookRotation && timer >= 0.05f)
        {
            transform.rotation = Quaternion.LookRotation(thisRigidbody.velocity);
        }

        //CheckCollision(previousPosition);

        previousPosition = transform.position;
    }

    void CheckCollision(Vector3 prevPos)
    {
        RaycastHit hit;
        Vector3 direction = transform.position - prevPos;
        Ray ray = new Ray(prevPos, direction);
        float dist = Vector3.Distance(transform.position, prevPos);
        if (Physics.Raycast(ray, out hit, dist))
        {
            transform.position = hit.point;
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            Vector3 pos = hit.point;
            Instantiate(impactPrefab, pos, rot);
            if (!explodeOnTimer && Missile == false)
            {
                Destroy(gameObject);
            }
            else if (Missile == true)
            {
                thisCollider.enabled = false;
                particleKillGroup.SetActive(false);
                thisRigidbody.velocity = Vector3.zero;
                Destroy(gameObject, 5);
            }

        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "FX" && collision.gameObject.tag != "Bullets" && collision.gameObject.tag != "Plane")
        {   
            Rigidbody hitRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize();

            //hitRigidbody.AddForce(direction * pushForce, ForceMode.Force);

            if (collision.gameObject.tag == "DestructableObstacles")
            {
                DestructableObstacles obstacle = collision.gameObject.GetComponent<DestructableObstacles>();
                obstacle.OnHit(damage);
            }
            else if(collision.gameObject.tag == "Boss")
            {
                BossController boss = collision.gameObject.GetComponent<BossController>();
                boss.OnHit(damage);
            }

            //ContactPoint contact = collision.contacts[0];
            //Vector3 position = transform.position;
            //Quaternion rot = Quaternion.FromToRotation(Vector3.forward, position.normal);
            //if (ignorePrevRotation)
            //{
            //    rot = Quaternion.Euler(0, 0, 0);
            //}
            //Vector3 pos = contact.point;
            
            Instantiate(impactPrefab, transform.position, transform.rotation);
            if (!explodeOnTimer && Missile == false)
            {
                Destroy(gameObject);
            }
            else if (Missile == true)
            {

                thisCollider.enabled = false;
                particleKillGroup.SetActive(false);
                thisRigidbody.velocity = Vector3.zero;

                Destroy(gameObject, 5);

            }
        }
    }
    IEnumerator Suicide(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    void Explode()
    {
        Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }

}