using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSpeaker : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource speaker;

    private void Start()
    {
        speaker = gameObject.GetComponent<AudioSource>();
    }
    public void Play()
    {
        speaker.clip = clips[Random.Range(0, 3)];
        speaker.Play();
    }
}
