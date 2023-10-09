using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float followAcceleration;
    public float followSpeed;
    public float followRadius;
    public float followPortion;
    private float velocityX;
    private float velocityY;
    private Vector3 initialPosition;
    private float initialY;
    void Start()
    {
        initialPosition = transform.position;
        Vector3 direction = player.transform.position - transform.position;
        initialY = transform.position.y - direction.y;
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(initialPosition, player.transform.position);
        Vector3 direction = player.transform.position - transform.position;
        if (distance <= followRadius)
        {
            //transform.position += new Vector3(direction.x * followPortion, direction.y * followPortion, direction.z * followPortion);
            transform.position = Vector3.Lerp(transform.position, new Vector3(initialPosition.x + direction.x * followPortion, initialPosition.y + direction.y * followPortion + initialY - 3, initialPosition.z), Time.deltaTime * followAcceleration);
        }
    }
}
