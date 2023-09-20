using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class MissileMovement : MonoBehaviour
{   
    //Rigidbodies
    public Rigidbody missile;
    public Rigidbody target;
    //Speeds
    public float speed = 5f;
    public float rotationSpeed = 30f;
    //Predictions
    private float maxDistancePredict = 100;
    private float minDistancePredict = 5;
    private float maxTimePrediction = 5;
    private Vector3 standardPrediction, deviatedPrediction;
    //Deviations
    private float deviationAmount = 50;
    private float deviationSpeed = 2;

    private void FixedUpdate() 
    {
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
        var predictionTime = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);

        standardPrediction = target.position + target.velocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * leadTimePercentage;

        deviatedPrediction = standardPrediction + predictionOffset;
    }
}
