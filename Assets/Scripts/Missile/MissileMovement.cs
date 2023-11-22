using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class MissileMovement : MonoBehaviour
{   
    //Rigidbodies
    public Rigidbody missile;
    //Speeds
    public float speed = 5f;
    public float rotationSpeed = 30f;
    //Targets
    public ParticleSystem Explode;
    public int Damage;
    public GameObject target;
    //Radar Range
    public float maxDetectionRange = 100f;
    public float detectionAngle = 45f;
    //Durations
    public float LifeTime = 3f;
    //Predictions
    private float maxDistancePredict = 100;
    private float minDistancePredict = 5;
    private float maxTimePrediction = 5;
    private Vector3 standardPrediction, deviatedPrediction;
    //Deviations
    private float deviationAmount = 50;
    private float deviationSpeed = 2;
    private float startTime;
    private float currentTime;

    private void Start()
    {
        startTime = Time.deltaTime;
        currentTime = startTime;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= startTime + LifeTime)
        {
            ParticleSystem effect = Instantiate(Explode, gameObject.transform.position, Quaternion.identity);
            effect.Play();
            Destroy(gameObject);
        }
    }
    private void FixedUpdate() 
    {
        if(target == null)
        {
            return;
        }
        missile.velocity = transform.forward * speed;

        var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, target.transform.position));

        PredictMovement(leadTimePercentage);

        //AddDeviation(leadTimePercentage);
        if (IsTargetWithinDetectionCone(target.transform))
        {
            RotateRocket();
        }
    }
    
    private void RotateRocket()
    {
        var heading = standardPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        missile.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime));
    }

    private void PredictMovement(float leadTimePercentage)
    {
        if (target == null && target.GetComponent<Rigidbody>() != null)
        {
            return;
        }

        var predictionTime = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);

        standardPrediction = target.transform.position + target.GetComponent<Rigidbody>().velocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * leadTimePercentage;

        deviatedPrediction = standardPrediction + predictionOffset;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == target)
        {
            if (collision.gameObject.tag == "DestructableObstacles")
            {
                DestructableObstacles obstacle = collision.gameObject.GetComponent<DestructableObstacles>();
                obstacle.OnHit(Damage);
                Boom();
            }
            else if (collision.gameObject.tag == "IndestructableObstacles")
            {
                //IndustructableObstacles obstacle = collision.gameObject.GetComponent<IndustructableObstacles>();
                //obstacle.OnHit(Damage);
                ParticleSystem effect = Instantiate(Explode, gameObject.transform.position, Quaternion.identity);
                effect.Play();
                Boom();
            }
            else if (collision.gameObject.tag == "Plane")
            {
                PlaneHealth plane = collision.gameObject.GetComponent<PlaneHealth>();
                plane.ReduceHealth(Damage, gameObject.transform.position);
                Boom();
            }
            else if (collision.gameObject.tag == "Boss")
            {
                BossController boss = collision.gameObject.GetComponent<BossController>();
                boss.OnHit(Damage);
                Boom();
            }
        }
    }

    private void Boom()
    {
        ParticleSystem effect = Instantiate(Explode, gameObject.transform.position, Quaternion.identity);
        effect.Play();
        Destroy(gameObject);
    }

    private bool IsTargetWithinDetectionCone(Transform target)
    {
        Vector3 vectorToTarget = target.position - transform.position;

        // Check if the target is within the detection range
        if (vectorToTarget.sqrMagnitude <= maxDetectionRange * maxDetectionRange)
        {
            // Normalize the vectorToTarget to use it in the dot product calculation
            vectorToTarget.Normalize();

            // Calculate the dot product
            float dotProduct = Vector3.Dot(transform.forward, vectorToTarget);

            // Convert the dot product to an angle
            float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

            // If the angle is within the detection cone's angle, the target is detected
            if (angle <= detectionAngle)
            {
                return true;
            }
        }

        // Target is not within the detection cone
        return false;
    }
}
