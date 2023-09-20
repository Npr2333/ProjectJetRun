using UnityEngine;
using UnityEngine.UI;

public class PlaneShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public Rigidbody m_Shell;
    public ParticleSystem m_flare;
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;

    public float RPM = 600f;

    private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired;

    // Add fire cooldown variables
    private float m_FireCooldown = 1f;  // 2 shots per second
    private float m_FireCooldownLeft = 0.0f;

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }

    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    private void Update()
    {
        // Decrease the time left for the next shot
        if(m_FireCooldownLeft > 0)
        {
            m_FireCooldownLeft -= Time.deltaTime;
        }

        // Fire button is pressed and cooldown has finished
        if(Input.GetButton(m_FireButton) && m_FireCooldownLeft <= 0)
        {
            Fire();
            m_FireCooldownLeft = m_FireCooldown/RPM;  // Start cooldown
        }
    }

    private void Fire()
    {
        // Instantiate and launch the shell.
        m_Fired = true;

        Rigidbody shell = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        ParticleSystem flare = Instantiate(m_flare, m_FireTransform.position, m_FireTransform.rotation) as ParticleSystem;
        
        flare.Play();

        shell.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}
