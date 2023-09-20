using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    
    public float speed = 10f;
    public float acceleration = 1f;
    public string x_axis = "Mouse X";
    public string y_axis = "Mouse Y";
    public float smoothTime = 0.3f;
    public float idleAccleration = 1f;
    private Rigidbody m_Rigidbody;
    private float currentSpeedX = 0f;
    private float currentSpeedY = 0f;
    private float velocityX = 0f;
    private float velocityY = 0f;

    private float health = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        //float moveX = Input.GetAxis(x_axis) * speed * Time.deltaTime;
        //float moveY = Input.GetAxis(y_axis) * speed * Time.deltaTime;

        //Vector3 new_position = m_Rigidbody.position + new Vector3(moveX, moveY, 0);
        //m_Rigidbody.MovePosition(new_position);

        //float moveX = Input.GetAxis(x_axis) * acceleration * Time.deltaTime;
        //float moveY = Input.GetAxis(y_axis) * acceleration * Time.deltaTime;

        //currentSpeedX = Mathf.SmoothDamp(currentSpeedX, moveX * speed, ref velocityX, smoothTime);
        //currentSpeedY = Mathf.SmoothDamp(currentSpeedY, moveY * speed, ref velocityY, smoothTime);

        //Vector3 new_position = m_Rigidbody.position + new Vector3(currentSpeedX, currentSpeedY, 0);
        //m_Rigidbody.MovePosition(new_position);
        float x_value = Mathf.Abs(Input.GetAxis(x_axis));
        float y_value = Mathf.Abs(Input.GetAxis(y_axis));

        float xForce = Input.GetAxis(x_axis) * acceleration;
        float yForce = Input.GetAxis(y_axis) * acceleration;

        if(x_value > 0.1 && y_value > 0.1)
        {
            m_Rigidbody.AddForce(new Vector3(xForce, yForce, 0), ForceMode.Force);
        }
        else if(x_value > 0.1)
        {
            m_Rigidbody.AddForce(new Vector3(xForce, -m_Rigidbody.velocity.y * idleAccleration, 0), ForceMode.Force);
        }
        else if(y_value > 0.1)
        {
            m_Rigidbody.AddForce(new Vector3(-m_Rigidbody.velocity.x * idleAccleration, yForce, 0), ForceMode.Force);
        }
        else 
        {
            m_Rigidbody.AddForce(-m_Rigidbody.velocity * idleAccleration);
        }



        
    }
}
