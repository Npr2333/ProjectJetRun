using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchBox : MonoBehaviour
{
    public bool isAutomated = true;
    public GameObject Box;
    public GameObject Parent;
    public ParticleSystem cast;
    public float launchTime;
    public float duration;
    public float castTime;
    //Box parameters
    public int damage = 50;
    public float targetScale = 50f;
    public float restScale = 0f;
    public float scaleTime = 1f;
    public Transform detectionTransform;
    public float detectionRate = 1f;
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
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= launchTime + initialTime && !launched && isAutomated)
        {
            StartCoroutine(LaunchObstacle());
            launched = true;
        }
    }

    IEnumerator LaunchObstacle()
    {
        float passedTime = 0f;
        GameObject shot = Instantiate(Box, detectionTransform.position, detectionTransform.rotation);
        ParticleSystem caster = Instantiate(cast, detectionTransform.position, detectionTransform.rotation);
        caster.transform.SetParent(transform);
        shot.transform.SetParent(Parent.transform);
        BoxScale box = shot.GetComponent<BoxScale>();
        box.setScalesAndTime(targetScale, restScale, scaleTime);
        Collider checker = shot.transform.Find("Indestructable Obstacle Sample").GetComponent<Collider>();
        DetectionCylinder detect = checker.GetComponent<DetectionCylinder>();
        detect.setDamage(damage);
        shot.SetActive(false);
        while (passedTime < duration + castTime)
        {
            if (passedTime < castTime && !casted)
            {
                caster.Play();
                casted = true;
                passedTime += Time.deltaTime;
                yield return null;
            }
            else if (passedTime < castTime && casted)
            {
                passedTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                if (caster)
                {
                    caster.Stop();
                    shot.SetActive(true);
                    box.changeScale();
                    Destroy(caster);
                }
                if (detectionCooldownLeft > 0)
                {
                    detectionCooldownLeft -= Time.deltaTime;
                    checker.enabled = false;
                }
                if (detectionCooldownLeft <= 0)
                {
                    detectionCooldownLeft = detectionCooldown / detectionRate;
                    checker.enabled = true;
                }
                passedTime += Time.deltaTime;
                yield return null;
            }

        }
    }

    public void Fire()
    {
        StartCoroutine(LaunchObstacle());
    }
}
