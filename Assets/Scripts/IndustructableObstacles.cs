using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustructableObstacles : Obstacles
{
     private void Start()
    {
        isDestructible = false;
        health = int.MaxValue;
    }

    public override void OnHit(Rigidbody player)
    {
        // Handle when player hits this thing
        // Ends the game here
    }
}
