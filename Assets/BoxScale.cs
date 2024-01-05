using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScale : MonoBehaviour
{
    public float targetScale = 10f;
    public float restScale = 0f;
    public float scaleTime = 1f;

    private void Start()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, restScale);
    }
    IEnumerator LerpScale()
    {
        float timeElapsed = 0;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = new Vector3(startScale.x, startScale.y, targetScale);

        while (timeElapsed < scaleTime)
        {
            // Lerp the scale
            transform.localScale = Vector3.Lerp(
                new Vector3(startScale.x, startScale.y, restScale),
                endScale,
                timeElapsed / scaleTime
            );

            // Increment the time elapsed
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void changeScale()
    {
        StartCoroutine(LerpScale());
    } 


    public void setScalesAndTime(float target, float rest, float time)
    {
        targetScale = target;
        restScale = rest;
        scaleTime = time;
    }
}
