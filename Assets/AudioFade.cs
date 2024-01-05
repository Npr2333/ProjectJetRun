using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    public float fadeTime = 3f;
    private AudioSource speaker;

    private void Start()
    {
        speaker = gameObject.GetComponent<AudioSource>();
    }
    
    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(Out());
    }
    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(In());
    }
    IEnumerator Out()
    {
        float timer = 0;
        while (timer < fadeTime)
        {
            float inital = speaker.volume;
            float tempVol = Mathf.Lerp(inital, 0, timer / fadeTime);

            speaker.volume = tempVol;
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator In()
    {
        float timer = 0;
        while (timer < fadeTime)
        {
            float inital = speaker.volume;
            float tempVol = Mathf.Lerp(inital, 1, timer / fadeTime);

            speaker.volume = tempVol;
            timer += Time.deltaTime;
            yield return null;
        }
    }

}
