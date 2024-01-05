using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage1Director : MonoBehaviour
{
    //This class is ment to handle the specific update logic of Stage1
    //public AudioSource musicSpeaker;
    public AudioSource dialogueSpeaker;
    public EventEntry[] Stage1Events;
    private float currentTime = 0f;
    private bool isUpdating = false;
    IEnumerator ProcessEvents(EventEntry[] events, GameManager.GameState state = GameManager.GameState.Dummy)
    {
        foreach (var entry in events)
        {
            yield return new WaitUntil(() => currentTime >= entry.delay);

            if (entry.clip != null)
            {
                dialogueSpeaker.clip = entry.clip;
                dialogueSpeaker.Play();
            }

            entry.action?.Invoke();
        }
        //if (state != GameManager.GameState.Dummy)
        //{
        //    GameManager gm = FindObjectOfType<GameManager>();
        //    gm.SetCurrentState(state);
        //}
    }

    public void DirectStage1()
    {
        isUpdating = true;
        currentTime = 0;
        StartCoroutine(ProcessEvents(Stage1Events, GameManager.GameState.Transition1));
    }

    public void StopDirect()
    {
        dialogueSpeaker.Stop();
        isUpdating = false;
        currentTime = 0;
        StopCoroutine("ProcessEvents");
    }

    private void Update()
    {
        if (isUpdating)
        {
            currentTime += Time.deltaTime;
        }
        //Debug.Log(currentTime);
    }
}
