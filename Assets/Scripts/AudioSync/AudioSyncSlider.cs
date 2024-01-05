using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioSyncSlider : MonoBehaviour
{
    public List<Slider> sliderSet = new List<Slider>();
    public AudioSource speaker;
    public float beatValue;
    public float restValue;
    public float intensity = 1000f;
    public float rot_duration = 1f;
    public float timeToBeat;
    private float[] m_audioSpectrum;
    private float[] buffer;
    private float[] bufferDecrease;
    private Slider currentSlider;
    private float _curr = 0f;
    private float _percent = 0f;
    private int shiftIndex = 0;
    private void Update()
    {
        // get the data
        //AudioListener.GetSpectrumData(m_audioSpectrum, 0, FFTWindow.Hamming);
        speaker?.GetSpectrumData(m_audioSpectrum, 0, FFTWindow.Hamming);
        // assign spectrum value
        // this "engine" focuses on the simplicity of other classes only..
        // ..needing to retrieve one value (spectrumValue)
        Buffer();
        updateSliders();
        if (_curr > rot_duration)
        {
            _curr = 0f;
            _percent = 0f;
            shiftIndex = 0;
        }
        else
        {
            _curr += Time.deltaTime;
            _percent = _curr / rot_duration;
            shiftIndex = (int)(_percent * sliderSet.Count() - 1);
        }
        //Debug.Log(shiftIndex);

    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            sliderSet.Add(child.GetComponent<Slider>());
        }
        buffer = new float[sliderSet.Count()];
        bufferDecrease = new float[sliderSet.Count()];
        /// initialize buffer
        m_audioSpectrum = new float[sliderSet.Count()];
        m_audioSpectrum.CopyTo(buffer, 0);
    }

    private void Buffer()
    {
        for (int i = 0; i < m_audioSpectrum.Count(); i++)
        {
            if (m_audioSpectrum[i] > buffer[i])
            {
                buffer[i] = m_audioSpectrum[i];
                bufferDecrease[i] = 0.005f;
            }
            if (m_audioSpectrum[i] < buffer[i])
            {
                buffer[i] -= bufferDecrease[i];
                bufferDecrease[i] = 1.2f;
            }
        }
    }
    private void updateSliders()
    {
        for (int i = 0; i < buffer.Count(); i++)
        {
            //currentSlider = sliderSet[i];
            //StartCoroutine(TweakSliders(m_audioSpectrum[i] * 100, i));
            int index = (i + shiftIndex) % sliderSet.Count();
            sliderSet[i].value = Mathf.Max(Mathf.Min(buffer[index] * intensity, 1) * beatValue, restValue);
            //if (m_audioSpectrum[index] * intensity <= 0.1f)
            //{
            //    StartCoroutine(TweakSliders(0, i));
            //}
            //else
            //{
            //    StartCoroutine(TweakSliders(Mathf.Min(m_audioSpectrum[index] * intensity, 1), i));
            //}
        }
    }

    IEnumerator TweakSliders(float _target, int i)
    {
        float _curr = sliderSet[i].value;
        float _initial = sliderSet[i].value;
        float _timer = 0;

        while (_curr != _target)
        {
            _curr = Mathf.Lerp(_initial, _target, _timer / timeToBeat);
            _timer += Time.deltaTime;

            sliderSet[i].value = _curr;

            yield return null;
        }
    }
}
