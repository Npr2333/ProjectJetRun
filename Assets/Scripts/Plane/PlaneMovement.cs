using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

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
    public float rotationResetSpeed = 0.5f;
    private Quaternion targetRotation;
    private Rigidbody m_Rigidbody;
    private float currentSpeedX = 0f;
    private float currentSpeedY = 0f;
    private float velocityX = 0f;
    private float velocityY = 0f;
    private float velocityZ = 0f;
    private bool isMoving = false;
    private float health = 10.0f;
    private float counter = 0;
    public bool topDown = false;
    private bool takeInput = true;
    public bool plannedMoving = false;
    public Transform plannedPosition;
    private float centralSpeed = 0f;
    private Vector3 previousPosition;
    private Vector3 currentSpeed;
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
        
        if(!CheckDash() && takeInput && !plannedMoving)
        {
            Move();
        }

        if (plannedMoving && plannedPosition != null)
        {
            toPosition(plannedPosition.transform.position);
        }
    }
    private void LateUpdate()
    {
        if (takeInput)
        {
            if (isMoving && !topDown)
            {
                targetRotation = Quaternion.Euler(pitchDegree * -Input.GetAxis(y_axis), 0, rollDegree * -Input.GetAxis(x_axis));
            }
            else if (isMoving)
            {
                targetRotation = Quaternion.Euler(pitchDegree * Input.GetAxis(y_axis), 0, rollDegree * -Input.GetAxis(x_axis));
            }
            else
            {
                targetRotation = Quaternion.identity;
            }
            planeRotator.transform.rotation = Quaternion.Lerp(planeRotator.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            float v_x = _controller.velocity.x;
            float v_y = _controller.velocity.y;
            targetRotation = Quaternion.Euler(pitchDegree * (-v_y / speed), 0, rollDegree * (-v_x / speed));
            planeRotator.transform.rotation = Quaternion.Lerp(planeRotator.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

    }

    public void Move()
    {
        if(!topDown)
        {
            velocityX = Mathf.Lerp(velocityX, Input.GetAxis(x_axis) * speed, acceleration * Time.deltaTime);
            velocityY = Mathf.Lerp(velocityY, Input.GetAxis(y_axis) * speed, acceleration * Time.deltaTime);
            Vector3 moveX = transform.right * velocityX * Time.deltaTime;
            Vector3 moveY = transform.up * velocityY * Time.deltaTime;
            _controller.Move(moveX + moveY);
            return;
        }
        else
        {
            velocityX = Mathf.Lerp(velocityX, Input.GetAxis(x_axis) * speed, acceleration * Time.deltaTime);
            velocityZ = Mathf.Lerp(velocityZ, Input.GetAxis(y_axis) * speed, acceleration * Time.deltaTime);
            Vector3 moveX = transform.right * velocityX * Time.deltaTime;
            Vector3 moveZ = transform.forward * velocityZ * Time.deltaTime;
            _controller.Move(moveX + moveZ);
            return;
        }

    }

    public bool CheckDash()
    {   
        if (!topDown && takeInput)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.A))
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

            //if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            //{
            //    animate.SetTrigger("DashUp");
            //    StartCoroutine(Dash("Up"));
            //    return true;
            //}
        }
        else if(takeInput)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.A))
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

            //if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            //{
            //    animate.SetTrigger("DashUp");
            //    StartCoroutine(Dash("Forward"));
            //    return true;
            //}
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
            else if(direction == "Forward")
            {
                //animate.ResetTrigger("DashUp");
                velocityZ = Mathf.Lerp(velocityZ, DashSpeed, acceleration * Time.deltaTime);
                Vector3 moveZ = transform.forward * velocityZ * Time.deltaTime;
                _controller.Move(moveZ);
            }


            yield return null;
        }
    }

    //Setters
    public void setTopDown()
    {
        topDown = !topDown;
    }

    public void setInputStatus(bool status)
    {
        takeInput = status;
    }

    public void MoveToPosition(Transform target)
    {
        plannedPosition = target;
        plannedMoving = true;

    }
    
    private void toPosition(Vector3 newPosition)
    {
        Vector3 direction = (newPosition - transform.position).normalized;

        // Calculate a move vector based on speed and deltaTime
        Vector3 move = direction * speed * Time.deltaTime;

        // Use MoveTowards to get a new position closer to the target
        Vector3 position = Vector3.MoveTowards(transform.position, newPosition, move.magnitude);

        // Move the CharacterController by the move vector
        _controller.Move(move);
        // Check if the player has reached the target position (or is very close to it)
        if (Vector3.Distance(transform.position, newPosition) < 0.2f)
        {
            plannedMoving = false;
            StartCoroutine(resetVelocity());
        }
    }

    IEnumerator resetVelocity()
    {
        Vector3 inital = _controller.velocity;
        float timer = 0f;
        while (_controller.velocity != Vector3.zero)
        {
            Vector3 move = Vector3.Lerp(inital, Vector3.zero, timer / rotationResetSpeed);
            timer += Time.deltaTime;
            _controller.Move(move * Time.deltaTime);
            yield return null;
        }
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public void takeSpeedInput(float inputSpeed)
    {
        centralSpeed = inputSpeed;
    }

    public void resetStatus()
    {
        planeRotator.transform.rotation = Quaternion.identity;
        topDown = false;
    }
    private void calculatePlaneSpeed()
    {
        if (_controller!= null)
        {
            // Calculate the speed as the distance covered since the last frame
            Vector3 deltaPosition = transform.position - previousPosition;
            currentSpeed = deltaPosition / Time.deltaTime;

            // Update the previous position for the next frame
            previousPosition = transform.position;
        }
    }

    public Vector3 getPlaneSpeed()
    {
        return currentSpeed;
    }
}
