using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSLWarner : MonoBehaviour
{
    public RWRTrigger rwrTrigger;
    public AudioSource speaker;
    public string tagToKeep;
    private List<GameObject> threatList = new List<GameObject>();
    private List<GameObject> innerList = new List<GameObject>();
    private void Update()
    {
        threatList = rwrTrigger.GetObjectsInTrigger();
        foreach (GameObject threat in threatList)
        {
            if (threat.tag == "EnemyMissile" && !innerList.Contains(threat))
            {
                innerList.Add(threat);
            }
        }
        if (innerList.Count > 0)
        {
            if (!speaker.isPlaying)
            {
                speaker.Play();
            }
        }
        else
        {
            speaker.Stop();
        }

        innerList.Clear();
    }
}
