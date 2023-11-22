using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;


[Serializable]
public class EventEntry
{
    public AudioClip clip;
    public float delay;
    public UnityEvent action;
}
public class Stage2Director : MonoBehaviour
{
    //This class is ment to handle the specific update logic of Transition1/Stage2/Transition2
    public AudioSource musicSpeaker;
    public AudioSource dialogueSpeaker;
    public AudioClip stage2Music;
    public EventEntry[] Transition1Events;
    public EventEntry[] Transition2Events;
    public EventEntry[] Stage2Events;
    public EventEntry[] Stage3Events;
    public void DirectTransition1()
    {
        Debug.Log("In Transition 1");
        if (stage2Music)
        {
            musicSpeaker.clip = stage2Music;
            musicSpeaker.Play();
        }
        StartCoroutine(ProcessEvents(Transition1Events, GameManager.GameState.Stage2));
    }

    public void DirectStage2()
    {
        StartCoroutine(ProcessEvents(Stage2Events));
    }

    public void DirectStage3()
    {
        StartCoroutine(ProcessEvents(Stage3Events));
    }
    public void DirectTransition2()
    {
        Debug.Log("In Transition 2");
        StartCoroutine(ProcessEvents(Transition2Events, GameManager.GameState.Stage3));
    }

    IEnumerator ProcessEvents(EventEntry[] events, GameManager.GameState state = GameManager.GameState.Dummy)
    {
        foreach (var entry in events)
        {
            yield return new WaitForSeconds(entry.delay);

            if (entry.clip != null)
            {
                dialogueSpeaker.clip = entry.clip;
                dialogueSpeaker.Play();
            }

            entry.action?.Invoke();
        }
        if (state != GameManager.GameState.Dummy)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.SetCurrentState(state);
        }
    }

    public void setToTransition2()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.SetCurrentState(GameManager.GameState.Transition2);
    }
}