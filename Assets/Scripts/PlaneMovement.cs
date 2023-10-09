using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{

    private CharacterController _controller;
    Animator animate;
    public float DashTime = 0.3f;
    public float speed = 10f;
    public float DashSpeed = 500f;
    public float acceleration = 1f;
    public string x_axis = "Mouse X";
    public string y_axis = "Mouse Y";
    public float smoothTime = 0.3f;
    public float idleAccleration = 1f;
    public GameObject planeRotator;
    public GameObject planeModel;
    public float rollDegree = 15f;
    public float pitchDegree = 15f;
    public float rotationSpeed = 30f;
    private Quaternion targetRotation;
    private Rigidbody m_Rigidbody;
    private float currentSpeedX = 0f;
    private float currentSpeedY = 0f;
    private float velocityX = 0f;
    private float velocityY = 0f;
    private bool isMoving = false;
    private float health = 10.0f;
    private float counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
        animate = planeModel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Input.GetAxis(x_axis)) > 0.2 || Mathf.Abs(Input.GetAxis(y_axis)) > 0.2)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if(!CheckDash())
        {
            Move();
        }
        if(counter == 2)
        {
            Debug.Log("Hi");
        }
    }
    private void LateUpdate()
    {
        if (isMoving)
        {
            targetRotation = Quaternion.Euler(pitchDegree * -Input.GetAxis(y_axis), 0, rollDegree * -Input.GetAxis(x_axis));
        }
        else
        {
            targetRotation = Quaternion.identity;
        }
        planeRotator.transform.rotation = Quaternion.Lerp(planeRotator.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
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
        //float x_value = Mathf.Abs(Input.GetAxis(x_axis));
        //float y_value = Mathf.Abs(Input.GetAxis(y_axis));

        //float xForce = Input.GetAxis(x_axis) * acceleration;
        //float yForce = Input.GetAxis(y_axis) * acceleration;

        //if(x_value > 0.1 && y_value > 0.1)
        //{
        //    m_Rigidbody.AddForce(new Vector3(xForce, yForce, 0), ForceMode.Force);
        //}
        //else if(x_value > 0.1)
        //{
        //    m_Rigidbody.AddForce(new Vector3(xForce, -m_Rigidbody.velocity.y * idleAccleration, 0), ForceMode.Force);
        //}
        //else if(y_value > 0.1)
        //{
        //    m_Rigidbody.AddForce(new Vector3(-m_Rigidbody.velocity.x * idleAccleration, yForce, 0), ForceMode.Force);
        //}
        //else 
        //{
        //    m_Rigidbody.AddForce(-m_Rigidbody.velocity * idleAccleration);
        //}
        velocityX = Mathf.Lerp(velocityX, Input.GetAxis(x_axis) * speed, acceleration * Time.deltaTime);
        velocityY = Mathf.Lerp(velocityY, Input.GetAxis(y_axis) * speed, acceleration * Time.deltaTime);
        Vector3 moveX = transform.right * velocityX * Time.deltaTime;
        Vector3 moveY = transform.up * velocityY * Time.deltaTime;
        //transform.Translate(moveX);
        //transform.Translate(moveY);

        //Vector3 movement = new Vector3(velocityX, velocityY, 0) * Time.deltaTime * speed;
        _controller.Move(moveX + moveY);
    }

    public bool CheckDash()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.A))
        {
            animate.SetTrigger("DashLeft");
            StartCoroutine(Dash("Left"));
            return true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.D))
        {
            animate.SetTrigger("DashRight");
            counter += 1;
            StartCoroutine(Dash("Right"));
            return true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            animate.SetTrigger("DashUp");
            StartCoroutine(Dash("Up"));
            return true;
        }

        return false;

    }

    IEnumerator Dash(string direction)
    {
        float startTime = Time.time;

        while(Time.time < startTime + DashTime)
        {
            if (direction == "Left")
            {
                //animate.ResetTrigger("DashLeft");
                velocityX = Mathf.Lerp(velocityX, -DashSpeed, acceleration * Time.deltaTime);
                Vector3 moveX = transform.right * velocityX * Time.deltaTime;
                _controller.Move(moveX);
            }
            else if(direction == "Right")
            {
                //animate.ResetTrigger("DashRight");
                velocityX = Mathf.Lerp(velocityX, DashSpeed, acceleration * Time.deltaTime);
                Vector3 moveX = transform.right * velocityX * Time.deltaTime;
                _controller.Move(moveX);
            }
            else if(direction == "Up")
            {
                //animate.ResetTrigger("DashUp");
                velocityY = Mathf.Lerp(velocityY, DashSpeed, acceleration * Time.deltaTime);
                Vector3 moveY = transform.up * velocityY * Time.deltaTime;
                _controller.Move(moveY);
            }


            yield return null;
        }
    }
}
