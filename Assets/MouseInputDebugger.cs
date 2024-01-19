using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputDebugger : MonoBehaviour
{
    void Update()
    {
        // Check for left mouse button press
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left Mouse Button Pressed");
        }

        // Check for right mouse button press
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right Mouse Button Pressed");
        }

        // Check for middle mouse button press
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Middle Mouse Button Pressed");
        }

        //// Check for mouse movement
        //if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        //{
        //    Debug.Log("Mouse Moved. X: " + Input.GetAxis("Mouse X") + ", Y: " + Input.GetAxis("Mouse Y"));
        //}

        // Check for scroll wheel
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            Debug.Log("Mouse Scroll: " + Input.GetAxis("Mouse ScrollWheel"));
        }
    }
}
