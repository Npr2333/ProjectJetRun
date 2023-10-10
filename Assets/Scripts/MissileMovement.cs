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

        RotateRocket();
    }
    
    private void RotateRocket()
    {
        var heading = standardPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        missile.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime));
    }

    private void PredictMovement(float leadTimePercentage)
    {
        if (target == null)
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
                Destroy(gameObject);
            }
            else if (collision.gameObject.tag == "IndestructableObstacles")
            {
                //IndustructableObstacles obstacle = collision.gameObject.GetComponent<IndustructableObstacles>();
                //obstacle.OnHit(Damage);
                ParticleSystem effect = Instantiate(Explode, gameObject.transform.position, Quaternion.identity);
                effect.Play();
                Destroy(gameObject);
            }
            else if (collision.gameObject.tag == "Plane")
            {
                PlaneHealth plane = collision.gameObject.GetComponent<PlaneHealth>();
                plane.ReduceHealth(Damage, gameObject.transform.position);
                Destroy(gameObject);
            }
        }

    }

}
