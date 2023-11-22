using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class CentralPackMovement : MonoBehaviour
{
    public float speed = 10f;
    public PlaneMovement plane;
    public bool isMoving = true;
    // Update is called once per frame
    void Update()
    {
        //if (isMoving)
        //{
        //transform.position += transform.forward * speed * Time.deltaTime;
        //if (plane)
        //{
        //    plane.takeSpeedInput(speed);
        //}
        //}
    }

    public void setPlane(GameObject planeObject)
    {
        plane = planeObject.GetComponent<PlaneMovement>();
    }

    public float getSpeed()
    {
        return speed;
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if (plane)
        {
            plane.takeSpeedInput(speed);
        }
    }


}
