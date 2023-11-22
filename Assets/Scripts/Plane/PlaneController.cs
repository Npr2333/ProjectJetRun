using UnityEngine;

public class PlaneController : MonoBehaviour
{   
    public float m_speed = 12f;
    public float idleSpeed = 0f;
    public AudioSource m_movementAudio;
    public AudioClip idle;
    public AudioClip afterburner;
    public float m_Acceleration = 5f;


    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_currentSpeed = 0f;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }

    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }

    private void Start()
    {
        m_MovementAxisName = "Mouse Y";
        m_TurnAxisName = "Horizontal";
    }

    private void Update()
    {
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        Move();

    }

    private void Move()
    {   
        //Adjust the position of the plane basd on the player's input.
        Vector3 movement = new Vector3(0,0,0);

        Vector3 idle = new Vector3(0,0,0);

        idle = transform.forward * idleSpeed * Time.deltaTime;

        movement = transform.forward * m_MovementInputValue * m_speed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement + idle);
    }


    
}
