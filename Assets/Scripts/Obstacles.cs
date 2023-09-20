using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacles : MonoBehaviour
{
    public int health; // 障碍物的健康值或耐久度
    public bool isDestructible; // 障碍物是否可摧毁

    protected virtual void Start()
    {
        // Instantiate the obstacle
    }

    protected virtual void Update()
    {
        
    }

    public abstract void OnHit(Rigidbody player); // When player hits the obstacle

    protected virtual void DestroyObstacle()
    {
        // Destroy the obstacle
        Destroy(gameObject);
    }
}
