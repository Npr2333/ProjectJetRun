using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class FollowCentral : MonoBehaviour
{
    public CentralPackMovement central;
    private float speed;
    private void Start()
    {
        if (central)
        {
            speed = central.getSpeed();
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
