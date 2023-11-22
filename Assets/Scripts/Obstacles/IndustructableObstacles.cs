using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class IndustructableObstacles : Obstacles
{
    [HideInInspector]public CharacterController controller;
    public PlaneHealth player;
    public int Damage = 100;
    public float destoryTime = 10f;
    private Vector3 ContactPoint;
    private Vector3 direction;
    private void Start()
    {
        //controller = GetComponent<CharacterController>();
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        if (gameManager && GameManager.Instance.player != null)
            player = GameManager.Instance.player.GetComponent<PlaneHealth>();

        isDestructible = false;
        health = int.MaxValue;
        StartCoroutine(Suicide(destoryTime));
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        if (direction != null)
        {
            transform.position += direction * Time.deltaTime;
        }
    }

    public override void OnHit(Collision collider)
    {
        // Handle when player hits this thing
        // Ends the game here
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
}
