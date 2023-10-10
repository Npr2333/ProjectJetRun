using System.Collections;
using System.Collections.Generic;
using Tarodev;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TargetingManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Transform missileTransform;
    public GameObject iconPrefab;
    public Vector2 hotSpot = new Vector2(16, 16);
    public LayerMask obstaclesLayer;
    public float selectionRadius = 1f;
    public float IconLerpTime = 1f;
    public GameObject missile;
    public AudioSource Speaker;
    public int missilesAtATime = 1;
    private GameObject currentTarget;
    private GameObject iconInstance;
    private Vector3 IconInitialPosition;
    private float currentLerpTime = 0;
    private bool locked = false;

    void Start()
    {
        //Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        trackTarget();
        //if (locked)
        //{
        //    Speaker.Play();
        //}
        //else
        //{
        //    Speaker.Stop();
        //}
    }

    public void trackTarget()
    {
        // Continuously check for a target under the mouse cursor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // SphereCast with the given radius
        if (Physics.SphereCast(ray, selectionRadius, out hit, Mathf.Infinity, obstaclesLayer))
        {
            currentTarget = hit.collider.gameObject;
            // Lock onto the target
            if (!locked)
            {
                locked = true;
                Speaker.Play();
                LockOnTarget(currentTarget);
            }
            else
            {
                MoveIconWithTarget(currentTarget);
            }
            Debug.Log("Locked" + Time.time);
            //Speaker.Play();
        }
        else
        {
            Destroy(iconInstance);
            currentTarget = null;
            locked = false;
            Speaker.Stop();
        }

        
        // Check for mouse click to fire a missile
        if (Input.GetMouseButtonDown(1) && currentTarget != null)
        {
            FireMissile(currentTarget);
        }
    }

    public void trackTarget2()
    {
        // Convert mouse position to world position
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));

        // Find all enemies within the lock-on radius of the mouse
        Collider[] hitColliders = Physics.OverlapSphere(worldPosition, selectionRadius, obstaclesLayer);

        // Find the closest enemy to the mouse
        float closestDistance = float.MaxValue;
        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(worldPosition, hitCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = hitCollider.gameObject;
            }
        }

        // Lock onto the closest enemy
        if (currentTarget != null)
        {
            LockOnTarget(currentTarget);
            Speaker.Play();
            Debug.Log("Locked");
        }

        // Check for mouse click to fire a missile
        if (Input.GetAxis("Fire2") > 0.1 && currentTarget != null)
        {
            FireMissile(currentTarget);
        }
    }

    public void LockOnTarget(GameObject Target)
    {
        iconInstance = Instantiate(iconPrefab);
        IconInitialPosition = Target.transform.position + new Vector3(0, -1, -1);
        iconInstance.transform.position = IconInitialPosition;
        

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > IconLerpTime)
        {
            currentLerpTime = IconLerpTime;
        }

        // Calculate the lerp value
        float perc = currentLerpTime / IconLerpTime;

        // Move the icon
        iconInstance.transform.position = Vector3.Lerp(IconInitialPosition, Target.transform.position, perc);
    }

    public void UnlockTarget()
    {

    }

    public void FireMissile(GameObject Target)
    {
        Quaternion rotation = Quaternion.identity;
        for(int i = 0; i < missilesAtATime; i++)
        {   

            GameObject rocket = Instantiate(missile, missileTransform.position, rotation * Quaternion.Euler(i * 360/missilesAtATime, i*360/missilesAtATime, i*360/missilesAtATime));
            MissileMovement msl = rocket.GetComponent<MissileMovement>();
            msl.target = Target;
        }
    }

    void MoveIconWithTarget(GameObject target)
    {
        iconInstance.transform.position = target.transform.position + new Vector3(0, 0, -1);
    }

}
