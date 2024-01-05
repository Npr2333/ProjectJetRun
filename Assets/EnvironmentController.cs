using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Splines.Interpolators;

public class EnvironmentController : MonoBehaviour
{
    public Transform lava;
    public Transform raisedPositionTransform;
    public float duration = 15f;
    public Color stage1SkyColor;
    public Color stage2SkyColor;
    public Vector2 stage2SunGradientOffset;
    public PostProcessVolume postProcessVolume;
    [ColorUsageAttribute(true, true)]
    public Color stage1BloomColor;
    public Color stage2BloomColor;
    private Material skyboxMaterial;
    private Color startSkyColor;
    public Vector2 startGradientOffset;
    private Vector3 startPosition;
    private Vector3 raisedPosition;
    private Bloom bloom;
    //private Color startBloomColor;
    void Start()
    {
        skyboxMaterial = RenderSettings.skybox;
        //startSkyColor = skyboxMaterial.GetColor("_UpperColor");
        //startGradientOffset = skyboxMaterial.GetVector("_SunGradientOffset");
        if (lava)
        {
            startPosition = lava.position;
        }
        raisedPosition = raisedPositionTransform.position;
        postProcessVolume.profile.TryGetSettings(out bloom);
        //startBloomColor = bloom.color;
    }

    IEnumerator LerpColor()
    {
        float elapsedTime = 0;
        bloom.color.value = stage2BloomColor;
        while (elapsedTime < duration)
        {
            Color tempSkyColor = Color.Lerp(stage1SkyColor, stage2SkyColor, elapsedTime / duration);
            skyboxMaterial.SetColor("_UpperColor", tempSkyColor);
            if (lava)
            {
                Vector3 tempPosition = Vector3.Lerp(startPosition, raisedPosition, elapsedTime / duration);
                lava.position = tempPosition;
            }
            //Vector2 tempOffset = Vector2.Lerp(startGradientOffset, stage2SunGradientOffset, elapsedTime / duration);
            //skyboxMaterial.SetVector("_SunGradientOffset", tempOffset);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void changeEnvironment()
    {
        StartCoroutine(LerpColor());
    }

    public void returnToOriginal()
    {
        bloom.color.value = stage1BloomColor;
        skyboxMaterial.SetColor("_UpperColor", stage1SkyColor);
        skyboxMaterial.SetVector("_SunGradientOffset", startGradientOffset);
    }
}
