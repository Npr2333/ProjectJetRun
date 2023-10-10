using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class BirdyCopterController : MonoBehaviour
{
    Animator animate;
    public GameObject HeliModel;
    public float range = 10f;
    //public float LaunchRange = 5f;
    public float RotationSpeed = 180f;
    public float MaxAOA = 30f;
    public Transform cannon;
    public Transform parent;
    public Transform target;
    public Transform m_FireTransform;
    public Rigidbody m_Shell;
    public ParticleSystem m_flare;
    public float launchForce = 10f;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float deviationAmount = 0f;
    public float RPM = 600f;

    // Add fire cooldown variables
    private float m_FireCooldown = 1f;  // 2 shots per second
    private float m_FireCooldownLeft = 0.0f;

    void Start()
    {
        animate = HeliModel.GetComponent<Animator>();
        animate.SetBool("IsFlying", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            LookAtPlayer();
        }
        //Debug.Log(cannon.transform.rotation.eulerAngles);
    }

    public void LookAtPlayer()
    {
        Vector3 directionToTarget = target.position - cannon.position;
        Vector3 currentRotation = cannon.transform.rotation.eulerAngles;
        Quaternion Rotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, directionToTarget.y, directionToTarget.z));
        Vector3 parentRotation = parent.transform.rotation.eulerAngles;
        Vector3 rotation = (Rotation * Quaternion.Inverse(Quaternion.Euler(0, parentRotation.y, parentRotation.z))).eulerAngles;
        Debug.Log(rotation);
        float x = (rotation.x > 180) ? rotation.x - 360 : rotation.x;
        float y = (rotation.y > 180) ? rotation.y - 360 : rotation.y;
        float z = (rotation.z > 180) ? rotation.z - 360 : rotation.z;
        x = Mathf.Clamp(x , -MaxAOA, MaxAOA) + parent.transform.rotation.eulerAngles.x;
        y = Mathf.Clamp(y, -MaxAOA, MaxAOA);
        z = Mathf.Clamp(z, -MaxAOA, MaxAOA);
        cannon.localRotation = Quaternion.RotateTowards(cannon.localRotation, Quaternion.Euler(-x,y,z), RotationSpeed * Time.deltaTime);
        //cannon.localRotation = Quaternion.Euler(-x, y, z);
        CheckFire();
    }

    public void CheckFire()
    {
        //if(gameObject.transform.position.z < target.transform.position.z)
        //{
            if (m_FireCooldownLeft > 0)
            {
                m_FireCooldownLeft -= Time.deltaTime;
            }

            // Fire button is pressed and cooldown has finished
            if (m_FireCooldownLeft <= 0)
            {
                Fire();
                m_FireCooldownLeft = m_FireCooldown / (RPM/60);  // Start cooldown
            }
        //}
    }

    private void Fire()
    {
        // Instantiate and launch the bullet.

        Rigidbody shell = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        ParticleSystem flare = Instantiate(m_flare, m_FireTransform.position, m_FireTransform.rotation) as ParticleSystem;

        flare.Play();
        Vector3 deviation = new Vector3(Random.Range(-deviationAmount,deviationAmount), Random.Range(-deviationAmount,deviationAmount), Random.Range(-deviationAmount, deviationAmount));
        shell.velocity = launchForce * m_FireTransform.forward + deviation;
        Debug.Log(shell.velocity);

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
    }

    public void setTarget(GameObject target)
    {
        this.target = target.transform;
    }
}
