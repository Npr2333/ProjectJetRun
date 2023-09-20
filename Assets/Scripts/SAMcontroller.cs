using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tarodev;
using UnityEngine;

public class SAMcontroller : MonoBehaviour
{
    public float range = 10f;
    public float LaunchRange = 5f;
    public float baseRotationSpeed = 30f;
    public float gimbalRotationSpeed = 30f;
    public Transform turretBase;
    public Transform target;
    public Transform LaunchPosition;
    public GameObject missile;
    private bool Launched = false;
    //public Transform turretGimbal;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if(distanceToPlayer < range)
            {
                LookAtPlayer();
            }
            if(distanceToPlayer < LaunchRange && !Launched)
            {
                LaunchMissile();
            }
        }
    }

    void LookAtPlayer()
    {
         Vector3 directionToTarget = target.position - turretBase.position;
            Quaternion baseRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
            Quaternion gimbalRotation = Quaternion.LookRotation(new Vector3(0,0,directionToTarget.z));

            turretBase.rotation = Quaternion.RotateTowards(turretBase.rotation, baseRotation, baseRotationSpeed * Time.deltaTime);
            //turretGimbal.localRotation = Quaternion.RotateTowards(turretGimbal.localRotation, gimbalRotation, gimbalRotationSpeed * Time.deltaTime);
    }

    void LaunchMissile()
    {
        GameObject LaunchedMissile = Instantiate(missile, LaunchPosition.position, LaunchPosition.rotation);
        Launched = true;
    }
}
