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
    public SpeakerPlay musicSpeaker;
    public AudioSource dialogueSpeaker;
    public AudioClip stage2Music;
    public EventEntry[] Transition1Events;
    public EventEntry[] Transition2Events;
    public EventEntry[] Stage2Events;
    public EventEntry[] Stage3Events;
    private float currentTime = 0f;
    private bool isUpdating = false;
    private bool isStopped = false;
    public void DirectTransition1()
    {
        isUpdating = true;
        isStopped = false;
        currentTime = 0;
        Debug.Log("In Transition 1");
        if (stage2Music)
        {
            musicSpeaker.Play();
        }
        StartCoroutine(ProcessEvents(Transition1Events, GameManager.GameState.Stage2));
    }

    public void DirectStage2()
    {
        isUpdating = true;
        isStopped = false;
        currentTime = 0;
        StartCoroutine(ProcessEvents(Stage2Events));
    }

    public void DirectStage3()
    {
        isUpdating = true;
        isStopped = false;
        currentTime = 0;
        StartCoroutine(ProcessEvents(Stage3Events));
    }
    public void DirectTransition2()
    {
        isUpdating = true;
        isStopped = false;
        currentTime = 0;
        Debug.Log("In Transition 2");
        StartCoroutine(ProcessEvents(Transition2Events, GameManager.GameState.Stage3));
    }

    IEnumerator ProcessEvents(EventEntry[] events, GameManager.GameState state = GameManager.GameState.Dummy)
    {
        foreach (var entry in events)
        {
            Debug.Log(Array.IndexOf(events, entry));
            if (isStopped)
            {
                break;
            }
            yield return new WaitUntil(() => currentTime >= entry.delay);

            if (entry.clip != null)
            {
                dialogueSpeaker.clip = entry.clip;
                dialogueSpeaker.Play();
            }

            entry.action?.Invoke();
        }
        isUpdating = false;
        if (state != GameManager.GameState.Dummy)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.SetCurrentState(state);
        }
        //currentTime = 0;
    }

    public void setToTransition2()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        Debug.Log(gm);
        gm.SetCurrentState(GameManager.GameState.Transition2);
    }

    public void stopDirect()
    {
        dialogueSpeaker.Stop();
        isUpdating = false;
        isStopped = true;
        StopAllCoroutines();
        currentTime = 0;
        musicSpeaker.Stop();
    }

    private void Update()
    {
        if (isUpdating)
        {
            currentTime += Time.deltaTime;
        }
    }


}