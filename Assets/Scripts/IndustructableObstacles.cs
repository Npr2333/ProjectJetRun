using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustructableObstacles : Obstacles
{
    public PlaneHealth player;
    public int Damage = 100;
    public float destoryTime = 10f;
    private Vector3 ContactPoint;

    private void Start()
    {
        if (GameManager.Instance.player != null)
            player = GameManager.Instance.player.GetComponent<PlaneHealth>();

        isDestructible = false;
        health = int.MaxValue;
        StartCoroutine(Suicide(destoryTime));
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
}
