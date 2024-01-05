using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class GradualBlackScreen : MonoBehaviour
{
    public float fadeTime = 1f;
    private PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;

    private void Start()
    {
        postProcessVolume = gameObject.GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out colorGrading);
    }

    public void Show()
    {
        StartCoroutine(fadeOut());
    }

    public void Hide()
    {
        StartCoroutine(fadeIn());
    }

    public void setToHide()
    {
        colorGrading.brightness.value = -100;
    }
    IEnumerator fadeOut()
    {
        float timer = 0;
        float initial = colorGrading.brightness;
        while (timer <= fadeTime)
        {
            colorGrading.brightness.value = Mathf.Lerp(initial, 1, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator fadeIn()
    {
        float timer = 0;
        float initial = colorGrading.brightness;
        while (timer <= fadeTime)
        {
            colorGrading.brightness.value = Mathf.Lerp(initial, -100, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
