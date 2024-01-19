using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneRailGunLaunch : MonoBehaviour
{
    public LaunchRailGun launchRailGun;
    public RgSliderController RgSliderController;
    public float castTime = 0.835f;
    public float duration = 3f;
    public float coolDownTime = 10f;
    private bool fired = false;
    private float currentTime = 0f;

    private void Start()
    {
        launchRailGun.setCastTime(castTime);
        launchRailGun.setDuration(duration);
    }
    void Update()
    {
        if (!enabled)
        {
            return;
        }
        if (!fired)
        {
            if (Input.GetMouseButtonDown(1))
            {
                launchRailGun.Fire(); 
                fired = true;
                currentTime = 0;
                RgSliderController.resetSlider(coolDownTime);
            }
        }
        else if (fired && currentTime < coolDownTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            fired = false;
        }
    }

    private void checkLaunch()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            launchRailGun.Fire();
        }
        fired = true;
    }
}
