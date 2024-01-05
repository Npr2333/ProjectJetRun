using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetingManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Transform missileTransform;
    public RaderConeTrigger rader;
    public GameObject iconPrefab;
    public Vector2 hotSpot = new Vector2(16, 16);
    public LayerMask obstaclesLayer;
    public LayerMask bossLayer;
    public int targetsAtATime = 1;
    public float selectionRadius = 1f;
    public float IconLerpTime = 1f;
    public GameObject missile;
    public AudioSource Speaker;
    public AudioSource launchSpeaker;
    public int missilesAtATime = 1;
    public float missileCoolDown = 1f;
    public GameObject centralBulletPack;
    public MslSliderController mslSliderController;
    private List<GameObject> targetList;
    private Dictionary<GameObject, bool> currentTargets = new Dictionary<GameObject, bool>();
    private GameObject currentTarget;
    private bool fired = false;
    private float currentTime = 0f;
    private int currentIndex = 0;
    void Start()
    {
        targetList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!fired)
        {
            trackTarget();
            currentTime = 0;
        }else if (fired && currentTime < missileCoolDown)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            fired = false;
        }
    }

    public void trackTarget()
    {
        ProcessClosestTargets();
        //Debug.Log(targetList[0]);
        if (currentTargets.Any())
        {
            List<GameObject> lst = new List<GameObject>();
            foreach (KeyValuePair<GameObject, bool> entry in currentTargets)
            {
                currentTarget = entry.Key;
                if (!entry.Value)
                {
                    //currentTargets[entry.Key] = true;
                    lst.Add(entry.Key);
                    Speaker.Play();
                    LockOnTarget();
                }
            }

            foreach(GameObject entry in lst)
            {
                currentTargets[entry] = true;
            }
        }
        else
        {
            currentTarget = null;
            Speaker.Stop();
        }

        // Check for mouse click to fire a missile
        if (Input.GetMouseButtonDown(1) && targetList.Any())
        {
            manageFire();
            launchSpeaker.Play();
            mslSliderController.resetSlider(missileCoolDown);
            fired = true;
        }


    }

    private void manageFire()
    {
        if (currentIndex >= targetList.Count)
        {
            currentIndex = 0;
        }
        for (int i = 0; i <= currentIndex; i++)
        {
            FireMissile(targetList[i]);
        }
        //FireMissile(targetList[currentIndex]);
        currentIndex++;  
    }

    public void LockOnTarget()
    {
        if (currentTarget.tag == "DestructableObstacles")
        {
            BirdyCopterController copter = currentTarget.GetComponent<BirdyCopterController>();
            copter.isLocked();
        }
        else if(currentTarget.tag == "Boss")
        {
            BossController bossController = currentTarget.GetComponent<BossController>();
            bossController.isLocked();
        }
    }

    public void FireMissile(GameObject Target)
    {
        Quaternion rotation = Quaternion.identity;
        for(int i = 0; i < missilesAtATime; i++)
        {   

            GameObject rocket = Instantiate(missile, missileTransform.position, rotation * Quaternion.Euler(i * 360/missilesAtATime, i*360/missilesAtATime, i*360/missilesAtATime));
            rocket.transform.parent = centralBulletPack.transform;
            MissileMovement msl = rocket.GetComponent<MissileMovement>();
            msl.target = Target;
        }
    }

    public void ProcessClosestTargets()
    {
        targetList = rader.GetObjectsInTrigger();
        targetList.RemoveAll(item => item == null);
        targetList = targetList.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude)
            .Take(targetsAtATime)
            .ToList();
        //Dictionary<GameObject, bool> temp = new Dictionary<GameObject, bool>();
        //Debug.Log(targetList);
        var temp = currentTargets.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);
        currentTargets.Clear();
        foreach (GameObject element in targetList)
        {
            if (temp.ContainsKey(element))
            {
                currentTargets.Add(element, temp[element]);
            }
            else
            {
                currentTargets.Add(element, false);
            }
        }

        List<GameObject> diffKeys1 = temp.Keys.Except(currentTargets.Keys).ToList();

        // Find keys in dictionary2 that are not in dictionary1
        List<GameObject> diffKeys2 = currentTargets.Keys.Except(temp.Keys).ToList();

        // Combine the lists if you want all unique keys from both dictionaries
        List<GameObject> combinedDiffKeys = diffKeys1.Union(diffKeys2).ToList();

        combinedDiffKeys.RemoveAll(item => item == null);
        foreach (GameObject entry in combinedDiffKeys)
        {
            if (entry.tag == "DestructableObstacles")
            {
                BirdyCopterController copter = entry.GetComponent<BirdyCopterController>();
                copter.notLocked();
            }
            else if(entry.tag == "Boss")
            {
                BossController bossController = entry.GetComponent<BossController>();
                bossController.notLocked();
            }
        }
    }

}
