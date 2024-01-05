using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class is to do the following:
/// Display the given subtitle on a renewal manner
/// Fade out the subtitles when the clip finishes
/// </summary>
public class SubtitleController : MonoBehaviour
{
    public AudioSource dialogueSpeaker;
    public float fadeTime = 0.2f;
    public float defaultDuration = 5f;
    public TextMeshProUGUI content;
    public Color firendColor;
    public Color foeColor;

    public void newSubtitle(Subtitle subtitle)
    {
        StopAllCoroutines();
        StartCoroutine(displaySubtitle(subtitle));
    }

    public void stopCaption()
    {
        StopAllCoroutines();
        dialogueSpeaker.Stop();
    }
    IEnumerator displaySubtitle(Subtitle subtitle)
    {
        float duration = defaultDuration;
        if (subtitle.clip)
        {
            dialogueSpeaker.Stop();
            dialogueSpeaker.clip = subtitle.clip;
            dialogueSpeaker.Play();
            duration = subtitle.clip.length;
        }
        
        if (subtitle.isFoe)
        {
            //addressor.color = foeColor;
            content.text = "<color=red>" + subtitle.name + "</color>" + ": " + subtitle.text;
        }
        else
        {
            //addressor.color = firendColor;
            content.text = "<color=blue>" + subtitle.name + "</color>" + ": " + subtitle.text;
        }

        Color tempContent = content.color;
        tempContent.a = 1f;
        content.color = tempContent;
        //Color tempName = addressor.color;
        //tempName.a = 1f;
        //addressor.color = tempName;

        yield return new WaitForSeconds(duration);

        float timer = 0;
        while (timer < fadeTime)
        {
            float inital = content.color.a;
            float tempAlpha = Mathf.Lerp(inital, 0, timer / fadeTime);

            Color tempC = content.color;
            tempC.a = tempAlpha;
            content.color = tempC;

            timer += Time.deltaTime;
            yield return null;
        }

    }
}
