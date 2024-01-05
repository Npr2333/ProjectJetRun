using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopNotification : MonoBehaviour
{
    public TextMeshProUGUI content;
    public float fadeTime = 1f;
    public float showTime = 3f;

    public void NewNotification(string notification)
    {
        content.text = notification;
        StopAllCoroutines();
        StartCoroutine(showNotification());
    }
    IEnumerator showNotification()
    {
        float timer = 0;
        while (timer < fadeTime)
        {
            float inital = content.color.a;
            float tempAlpha = Mathf.Lerp(inital, 1, timer / fadeTime);

            Color tempC = content.color;
            tempC.a = tempAlpha;
            content.color = tempC;

            timer += Time.deltaTime;
            yield return null;
        }
        Color tempContent = content.color;
        tempContent.a = 1f;
        content.color = tempContent;
        timer = 0;
        yield return new WaitForSeconds(showTime);
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
