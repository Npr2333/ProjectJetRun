using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObstacles : Obstacles
{
    private void Start()
    {
        isDestructible = true;
        health = 1; 
    }

    public override void OnHit(Rigidbody player)
    {
        // When player hits this thing
        health--;
        if (health <= 0)
        {
            DestroyObstacle();
        }
        // More functions to be added here
    }
}
