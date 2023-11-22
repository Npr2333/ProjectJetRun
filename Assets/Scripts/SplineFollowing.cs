using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineFollowing: MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f; 
    public Vector3 offset; 

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        //transform.position = followTransform.position;
        offset = transform.position - target.position;
    }

    private void Update()
    {   
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        // Quaternion targetRotation = target.rotation;
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime * Time.deltaTime);
    }

    public void setTarget(Transform newTarget)
    {
        target = newTarget;
        offset = transform.position - target.position;
    }
}
