using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera camera;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;
    private void Awake()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        //Debug.Log("Shaking");
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            float percentShake = shakeTimer / shakeTimerTotal;
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            Mathf.Lerp(startingIntensity, 0f, shakeTimer / shakeTimerTotal);
            //cinemachineBasicMultiChannelPerlin.m_AmplitudeGain *= percentShake;
            if (shakeTimer <=0)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }
}
