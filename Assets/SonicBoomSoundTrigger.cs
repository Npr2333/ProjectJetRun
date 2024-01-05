using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicBoomSoundTrigger : MonoBehaviour
{
    public string tagToCheck;
    public AudioSource audioSource;
    public AudioClip boomSFX;
    public ParticleSystem vapor;
    public CameraShake cameraInstance;
    public float shakeIntensity = 10f;
    public float shakeTime = 0.5f;
    public HUDController hudController;
    public bool isGun;
    public bool isMSL;
    public bool isCanvas;
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == tagToCheck || other.gameObject.tag == "Plane")
        {
            audioSource.clip = boomSFX;
            audioSource.Play();
            vapor.Play();
            if (cameraInstance)
            {
                cameraInstance.ShakeCamera(shakeIntensity, shakeTime);
            }
            if (hudController)
            {
                if (isGun)
                {
                    hudController.disableGun();
                    return;
                }
                if (isMSL)
                {
                    hudController.disableMSL();
                    return;
                }
                if (isCanvas)
                {
                    hudController.disableStage1Canvas();
                    return;
                }
            }
        }
    }
}
