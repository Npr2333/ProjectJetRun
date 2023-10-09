using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnController : MonoBehaviour
{   
    public AudioSource Speaker;
    //public AudioClip music;
    public float launchForce = 10f;
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
    private List<List<GameObject>> spawnPoints;
    private  List<List<double>> notes = StageOneNotes.notes;
    private float startTime;

    void Start()
    {
        // Initialize noteDataList from GeneratedCode.cs
        // You may need to adjust this line to correctly access the NoteData array from GeneratedCode.cs
        // Record the start time (you can also call this in another method when your specific event happens)
        startTime = Time.time;
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

            
        // Start the spawn coroutine
        Speaker.Play();
        StartCoroutine(SpawnObstaclesCoroutine());
    }

    IEnumerator SpawnObstaclesCoroutine()
    {
        foreach (var note in notes)
        {
            double targetTime = startTime + note[0];
            double waitTime = targetTime - Time.time;
            if (waitTime > 0)
                yield return new WaitForSeconds((float)waitTime); // Wait until the target time
            SpawnObstacle((int)note[3], (int)note[1], (int)note[2]);
        }
    }

    void SpawnObstacle(int type, int position, int layer)
    {
        GameObject prefab = (type == 0) ? destructibleObstaclePrefab : indestructibleObstaclePrefab;
        GameObject Obstacle = Instantiate(prefab, spawnPoints[layer][position].transform.position, Quaternion.identity);
        Rigidbody rb = Obstacle.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, 0, launchForce), ForceMode.Force);
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
}
