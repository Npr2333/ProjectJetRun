using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffectTrigger : MonoBehaviour
{
    public float spawnEffectTime = 2;
    //public float pause = 1;
    public AnimationCurve fadeIn;
    public AnimationCurve fadeOut;
    private AnimationCurve currentFade;
    private bool isFading = false;
    ParticleSystem ps;
    float timer = 0;
    Renderer _renderer;

    int shaderProperty;

    void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        _renderer = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        currentFade = fadeIn;
        var main = ps.main;
        main.duration = spawnEffectTime;

        ps.Play();

    }

    void Update()
    {
        if (timer < spawnEffectTime)
        {
            timer += Time.deltaTime;
        }
        else
        {

        }
        //else
        //{
        //    ps.Play();
        //    timer = 0;
        //}
        if (isFading)
        {
            if (timer < spawnEffectTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                isFading = false;
                timer = 0;
            }
        }
        _renderer.material.SetFloat(shaderProperty, currentFade.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));

    }
    public void setEffectTime(float num)
    {
        spawnEffectTime = num;
    }
    public void Construct()
    {
        isFading = true;
        ps.Play();
        currentFade = fadeOut;
    }

    public void Dissvolve()
    {
        isFading = true;
        ps.Play();
        currentFade = fadeIn;
    }
}
