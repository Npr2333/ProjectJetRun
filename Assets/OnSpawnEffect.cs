using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSpawnEffect : MonoBehaviour
{
    public ParticleSystem spawnEffect;
    public float effectTime = 0.5f;

    public void Start()
    {
        StartCoroutine(spawn());
    }
    IEnumerator spawn()
    {
        spawnEffect.Play();
        yield return new WaitForSeconds(effectTime);
        spawnEffect.Stop();
    }
}
