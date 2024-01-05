using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tarodev;
using UnityEngine;
using static ProceduralMeshGeneration;

public class SAMcontroller : MonoBehaviour
{
    public LaunchRailGun railGun;
    public float range = 10f;
    public float LaunchRange = 5f;
    public float baseRotationSpeed = 30f;
    public float gimbalRotationSpeed = 30f;
    public float activeMultiplier = 0.5f;
    public Transform turretBase;
    public Transform gimbal;
    public Transform target;
    public Transform LaunchPosition;
    public GameObject missile;
    public bool otherSide = false;
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
            float distanceToPlayer = (transform.position - target.position).z;
            float absDistance = Vector3.Distance(target.position, transform.position);
            if(absDistance < range)
            {
                LookAtPlayer();
                if (distanceToPlayer < LaunchRange && !Launched)
                {
                    LaunchMissile();
                }
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 directionToTarget = target.position - turretBase.position;
        Vector3 localDirectionToPlayer = target.position - gimbal.position;
        Quaternion baseRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, turretBase.up.y, directionToTarget.z));
        float degree = 0;
        if (!otherSide)
        {
            degree = Mathf.Atan2(localDirectionToPlayer.y, localDirectionToPlayer.x) * Mathf.Rad2Deg;
        }
        else
        {
            degree = Mathf.Atan2(localDirectionToPlayer.y, -localDirectionToPlayer.x) * Mathf.Rad2Deg;
        }
        //if (degree < 0)
        //{
        //    degree += 360;
        //}
        //else if (degree > 360)
        //{
        //    degree -= 360;
        //}

        Quaternion gimbalRotation = Quaternion.Euler(-degree, 0, 0);
        if (Launched)
        {
            //turretBase.rotation = Quaternion.RotateTowards(turretBase.rotation, baseRotation, baseRotationSpeed * activeMultiplier * Time.deltaTime);
            //turretGimbal.localRotation = Quaternion.RotateTowards(turretGimbal.localRotation, gimbalRotation, gimbalRotationSpeed * Time.deltaTime);
            //gimbal.localRotation = Quaternion.RotateTowards(gimbal.localRotation, gimbalRotation, baseRotationSpeed * Time.deltaTime);
            gimbal.localRotation = Quaternion.Lerp(gimbal.localRotation, gimbalRotation, baseRotationSpeed * activeMultiplier * Time.deltaTime);
            return;
        }
        //turretBase.localRotation = Quaternion.RotateTowards(turretBase.localRotation, baseRotation, baseRotationSpeed * Time.deltaTime);
        //turretGimbal.localRotation = Quaternion.RotateTowards(turretGimbal.localRotation, gimbalRotation, gimbalRotationSpeed * Time.deltaTime);
        //gimbal.localRotation = Quaternion.RotateTowards(gimbal.localRotation, gimbalRotation, baseRotationSpeed * Time.deltaTime);
        gimbal.localRotation = Quaternion.Lerp(gimbal.localRotation, gimbalRotation, baseRotationSpeed * Time.deltaTime);
    }

    void LaunchMissile()
    {
        railGun.Fire();
        Launched = true;
    }
}
