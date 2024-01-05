using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerPlay : MonoBehaviour
{
    public void Play()
    {
        foreach (Transform child in transform)
        {
            AudioSource subSpeaker = child.GetComponent<AudioSource>();
            subSpeaker.Play();
        }
    }

    public void Stop()
    {
        foreach (Transform child in transform)
        {
            AudioSource subSpeaker = child.GetComponent<AudioSource>();
            subSpeaker.Stop();
        }
    }
}
