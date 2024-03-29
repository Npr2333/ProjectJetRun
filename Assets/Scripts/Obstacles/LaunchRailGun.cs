using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class LaunchRailGun : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAutomated = true;
    public GameObject beam;
    public GameObject detectCylinder;
    public ParticleSystem cast;
    public float launchTime;
    public float duration;
    public float castTime;
    public Transform detectionTransform;
    public float detectionRate = 1f;
    public CameraShake cameraShake;
    public float shakeIntensity = 3f;
    public float shakeTime = 0.3f;
    private AudioSource speaker;
    public AudioClip chargeClip;
    public AudioClip fireClip;
    private float currentTime;
    private float initialTime;
    private float detectionCooldownLeft = 0f;
    private float detectionCooldown = 1f;
    private bool launched = false;
    private bool casted = false;
    void Start()
    {
        currentTime = Time.deltaTime;
        initialTime = Time.deltaTime;
        detectCylinder.SetActive(false);
        speaker = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
        currentTime += Time.deltaTime;
        if (currentTime >= launchTime + initialTime && ! launched && isAutomated)
        {
            StartCoroutine(launchBeam());
            launched = true;
        }
    }

    private IEnumerator launchBeam()
    {
        float passedTime = 0f;
        GameObject shot = Instantiate(beam, detectionTransform.position, detectionTransform.rotation);
        ParticleSystem caster = Instantiate(cast, detectionTransform.position, detectionTransform.rotation);
        caster.transform.SetParent(transform);
        shot.transform.SetParent(transform);
        //ParticleSystem RG_beam = shot.GetComponent<ParticleSystem>();
        //RG_beam.Play();
        //GameObject detection = Instantiate(detectCylinder, detectionTransform.position, detectionTransform.rotation);
        detectCylinder.SetActive(false);
        shot.SetActive(false);
        while(passedTime < duration + castTime)
        {   
            if(passedTime < castTime && !casted)
            {
                caster.Play();
                speaker.clip = chargeClip;
                speaker.Play();
                casted = true;
                passedTime += Time.deltaTime;
                yield return null;
            }
            else if(passedTime < castTime && casted)
            {
                passedTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                if (caster)
                {
                    caster.Stop();
                    speaker.Stop();
                    speaker.clip = fireClip;
                    speaker.Play();
                    cameraShake.ShakeCamera(shakeIntensity, shakeTime);
                }
                Destroy(caster);
                shot.SetActive(true);
                if (detectionCooldownLeft > 0)
                {
                    detectionCooldownLeft -= Time.deltaTime;
                    detectCylinder.SetActive(false);
                    //Debug.Log("On");
                }

                // Fire button is pressed and cooldown has finished
                if (detectionCooldownLeft <= 0)
                {
                    detectCylinder.SetActive(true);
                    //Debug.Log("Off");
                    detectionCooldownLeft = detectionCooldown / detectionRate;  // Start cooldown
                }
                passedTime += Time.deltaTime;
                casted = false;
                yield return null;
            }
         
        }
        if(passedTime >= duration)
        {
            shot.SetActive(false);
            //RG_beam.Stop();
            Destroy(shot);
            detectCylinder.SetActive(false);
        }
    }

    public void Fire()
    {
        StartCoroutine(launchBeam());
    }

    public void setCastTime(float num)
    {
        castTime = num;
    } 

    public void setDuration(float num)
    {
        duration = num;
    }
    
}
