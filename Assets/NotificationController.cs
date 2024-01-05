using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationController : MonoBehaviour
{
    public TextMeshProUGUI content;
    public float showTime = 3f;

    public void newNotification(string message)
    {
        StopAllCoroutines();
        StartCoroutine(displaySubtitle(message));
    }

    public void stopNotification()
    {
        StopAllCoroutines();
    }
    IEnumerator displaySubtitle(string message)
    {
        content.text = message;
        content.gameObject.SetActive(true);
        yield return new WaitForSeconds(showTime);
        content.gameObject.SetActive(false);

    }
}
