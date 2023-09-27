using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustructableObstacles : Obstacles
{
    public PlaneHealth player;
    public int Damage = 100;
    private Vector3 ContactPoint;

         private void Start()
    {
        isDestructible = false;
        health = int.MaxValue;
    }

    public override void OnHit(Collision collider)
    {
        // Handle when player hits this thing
        // Ends the game here
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
}
