using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Accessibility;

public class SpawnController : MonoBehaviour
{   
    public AudioSource Speaker;
    //public AudioClip music;
    public CentralPackMovement central;
    public GameObject centralBulletPack;
    public float launchSpeed = 10f;
    public float stageEndwaitSeconds = 0f;
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
    public GameObject destructibleObstaclePrefab;
    public GameObject indestructibleObstaclePrefab;
    public GameObject player;
    public bool isPlaceHolder = false;
    public GameObject Central;
    private CentralPackMovement centralMovement;
    private List<List<GameObject>> spawnPoints;
    private  List<List<double>> notes = StageOneNotes.notes;
    private float startTime;
    private GameManager gameManager;
    private float centralSpeed;

    void Start()
    {   
        if (isPlaceHolder)
        {
            notes = StageOneNotes.placeHolder;
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

        centralMovement = Central.GetComponent<CentralPackMovement>();
    }

    private void Update()
    {
        if (central)
        {
            centralSpeed = central.getSpeed();
        }
    }

    IEnumerator SpawnObstaclesCoroutine()
    {
        foreach (var note in notes)
        {
            double targetTime = startTime + note[0];
            double waitTime = targetTime - Time.time;
            if (waitTime > 0)
                yield return new WaitForSeconds((float)waitTime); 
            SpawnObstacle((int)note[3], (int)note[1], (int)note[2]);
        }

        yield return new WaitForSeconds(stageEndwaitSeconds);//Wait for some second before set the new stage

        gameManager.SetCurrentState(GameManager.GameState.Transition1);//Set the stage to transiton1
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
        }
        else
        {
            RG.setMovement(transform.forward * launchSpeed);
        }
        //rb.AddForce(new Vector3(0, 0, launchForce), ForceMode.Force);
        //if (type == 0)
        //{
        //    GameObject Obstacle = Instantiate(destructibleObstaclePrefab, spawnPoints[layer][position].transform.position, Quaternion.identity);
        //    DestructableObstacles script = Obstacle.GetComponent<DestructableObstacles>();
        //    script.setTarget(player);
        //    Rigidbody rb = Obstacle.GetComponent<Rigidbody>();
        //    rb.AddForce(new Vector3(0, 0, launchForce), ForceMode.Force);
        //}
        //else if(type == 1)
        //{
        //    GameObject Obstacle = Instantiate(indestructibleObstaclePrefab, spawnPoints[layer][position].transform.position, Quaternion.identity);
        //    IndustructableObstacles script = Obstacle.GetComponent<IndustructableObstacles>();
        //    script.setTarget(player);
        //    Rigidbody rb = Obstacle.GetComponent<Rigidbody>();
        //    rb.AddForce(new Vector3(0, 0, launchForce), ForceMode.Force);
        //}
    }

    public void setTarget(GameObject target)
    {
        player = target;
    }

    public void startSpawn()
    {
        startTime = Time.time;
        Speaker.Play();
        StartCoroutine(SpawnObstaclesCoroutine());
    }
}
