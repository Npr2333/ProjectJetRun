using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class AudioSyncBloom : AudioSyncer
{
    // Start is called before the first frame update
    public PostProcessVolume postProcessVolume;
    private Bloom bloom;
    public float beatIntensity;
    public float restIntensity;
    public float beatDiffusion;
    public float restDiffusion;
    void Start()
    {
        postProcessVolume.profile.TryGetSettings(out bloom);
    }
    private IEnumerator MoveToIntensity(float b_target)
    {
        float b_curr = bloom.intensity.value;
        float b_initial = b_curr;
        float _timer = 0;

        while (b_curr != b_target)
        {
            b_curr = Mathf.Lerp(b_initial, b_target, _timer / timeToBeat);
            _timer += Time.deltaTime;

            bloom.intensity.value = b_curr;

            yield return null;
        }

        m_isBeat = false;
    }

    private IEnumerator MoveToDiffusion(float d_target)
    {
        float d_curr = bloom.diffusion.value;
        float d_initial = d_curr;
        float _timer = 0;

        while (d_curr != d_target)
        {
            d_curr = Mathf.Lerp(d_initial, d_target, _timer / timeToBeat);
            _timer += Time.deltaTime;

            bloom.diffusion.value = d_curr;

            yield return null;
        }

        m_isBeat = false;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_isBeat) return;

        bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, restIntensity, restSmoothTime * Time.deltaTime);
        bloom.diffusion.value = Mathf.Lerp(bloom.diffusion.value, restDiffusion, restSmoothTime * Time.deltaTime);
    }

    public override void OnBeat()
    {
        base.OnBeat();

        StopCoroutine("MoveToIntensity");
        StopCoroutine("MoveToDiffusion");
        StartCoroutine(MoveToIntensity(beatIntensity));
        StartCoroutine(MoveToDiffusion(beatDiffusion));
    }

}
