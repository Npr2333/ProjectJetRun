using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.UIElements;

public class SpawnController : MonoBehaviour
{
    public SpeakerPlay speaker;
    public CentralPackMovement central;
    public GameObject centralBulletPack;
    public float launchSpeed = 10f;
    public float stageEndwaitSeconds = 0f;
    public Vector2 interval1;
    public GameObject spawnPoint11;
    public GameObject spawnPoint12;
    public GameObject spawnPoint13;
    public GameObject spawnPoint14;
    public GameObject spawnPoint21;
    public GameObject spawnPoint22;
    public GameObject spawnPoint23;
    public GameObject spawnPoint24;
    public GameObject spawnPoint31;
    public GameObject spawnPoint32;
    public GameObject spawnPoint33;
    public GameObject spawnPoint34;
    public GameObject spawnPoint01;
    public GameObject spawnPoint02;
    public GameObject spawnPoint03;
    public GameObject spawnPoint04;
    public GameObject destructibleObstaclePrefab;
    public GameObject indestructibleObstaclePrefab;
    public GameObject lavaBurstPrefab;
    public GameObject player;
    public bool isPlaceHolder = false;
    public GameObject Central;
    public ScoreQueue scoreQueue;
    private CentralPackMovement centralMovement;
    private List<List<GameObject>> spawnPoints;
    private List<GameObject> AltspawnPoints;
    private List<List<double>> notes;
    private float startTime;
    private GameManager gameManager;
    private double elapsedTime = 0;
    private bool isMagma = false;
    private bool isStopped = false;
    void Start()
    {   
        if (isPlaceHolder)
        {
            notes = StageOneNotes.placeHolder;
        }
        else
        {
            notes = StageOneNotes.GetEasyProcessedData();
        }
        gameManager = FindObjectOfType<GameManager>();
        List<GameObject>spawnPoints1 = new List<GameObject>
        {
            spawnPoint11,spawnPoint12,spawnPoint13,spawnPoint14
        };
        List<GameObject>spawnPoints2 = new List<GameObject>
        {
            spawnPoint21, spawnPoint22, spawnPoint23, spawnPoint24
        };
        List<GameObject>spawnPoints3 = new List<GameObject>
        {
            spawnPoint31, spawnPoint32, spawnPoint33, spawnPoint34
        };

        spawnPoints = new List<List<GameObject>>();
        spawnPoints.Add(spawnPoints1);
        spawnPoints.Add(spawnPoints2);
        spawnPoints.Add(spawnPoints3);

        AltspawnPoints = new List<GameObject>();
        AltspawnPoints.Add(spawnPoint01);
        AltspawnPoints.Add(spawnPoint02);
        AltspawnPoints.Add(spawnPoint03);
        AltspawnPoints.Add(spawnPoint04);

        centralMovement = Central.GetComponent<CentralPackMovement>();
    }

    private void Update()
    {
        //if (central)
        //{
        //    centralSpeed = central.getSpeed();
        //}
    }

    IEnumerator SpawnObstaclesCoroutine()
    {
        foreach (var note in notes)
        {
            if (isStopped)
            {
                break;
            }
            double conversion = 60.0 / 324.0;
            double targetTime = startTime + note[4] * conversion ;
            double waitTime = targetTime - Time.time;
            if (waitTime > 0)
                elapsedTime += waitTime;
                yield return new WaitForSeconds((float)waitTime);
            if (elapsedTime >= interval1[0] + startTime && elapsedTime <= interval1[1] + startTime)
            {
                //AltSpawnObstacle((int)note[1]);
            }
            else
            {
                SpawnObstacle((int)note[2], (int)note[0], (int)note[1]);
            }
            elapsedTime += Time.deltaTime;
        }

        yield return new WaitForSeconds(stageEndwaitSeconds);//Wait for some second before set the new stage

        gameManager.SetCurrentState(GameManager.GameState.Transition1);//Set the stage to transiton1

        //for (int i = 0; i < notes.Count; i++ )
        //{
        //    Debug.Log(elapsedTime);
        //    double targetTime = startTime + notes[i][0];
        //    double waitTime = targetTime - Time.time;
        //    if (waitTime > 0)
        //        elapsedTime += waitTime;
        //        yield return new WaitForSeconds((float)waitTime);
        //    SpawnObstacle((int)notes[i][3], (int)notes[i][1], (int)notes[i][2]);
        //    elapsedTime += Time.deltaTime;
        //}
    }

    void SpawnObstacle(int type, int position, int layer)
    {
        GameObject prefab = (type == 0) ? destructibleObstaclePrefab : indestructibleObstaclePrefab;
        GameObject Obstacle = Instantiate(prefab, spawnPoints[layer][position].transform.position, Quaternion.Euler(0,180,0));
        Obstacle.transform.parent = central.transform;
        //Rigidbody rb = Obstacle.GetComponent<Rigidbody>();
        BirdyCopterController heli = Obstacle.GetComponent<BirdyCopterController>();
        IndustructableObstacles RG = Obstacle.GetComponent<IndustructableObstacles>();
        if(heli)
        {
            heli.setTarget(player);
            heli.setMovement(transform.forward * launchSpeed);
            heli.setBulletPack(centralBulletPack);
            DestructableObstacles heli_O = Obstacle.GetComponent<DestructableObstacles>();
            heli_O.scoreQueue = this.scoreQueue;
        }
        else
        {
            RG.setMovement(transform.forward * launchSpeed);
            RG.scoreQueue = this.scoreQueue;
        }
    }

    void AltSpawnObstacle(int position)
    {
        GameObject prefab = lavaBurstPrefab;
        GameObject Obstacle = Instantiate(prefab, AltspawnPoints[position].transform.position, Quaternion.Euler(270, 0, 0));
        Obstacle.transform.parent = central.transform;
        LaunchRailGun lavaBurst = Obstacle.GetComponent<LaunchRailGun>();
        IndustructableObstacles RG = Obstacle.GetComponent<IndustructableObstacles>();
        RG.setMovement(transform.forward * launchSpeed);
    }

    public void setTarget(GameObject target)
    {
        player = target;
    }

    public void startSpawn()
    {
        startTime = Time.time - 1;
        elapsedTime = Time.time;
        isStopped = false;
        speaker.Play();
        StartCoroutine(SpawnObstaclesCoroutine());
    }

    public void stopSpawn()
    {
        isStopped = true;
        StopAllCoroutines();
        startTime = 0;
        elapsedTime = 0;
        speaker.Stop();
    }
}
