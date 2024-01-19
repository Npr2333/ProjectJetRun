using Kino;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HUDController : MonoBehaviour
{
    public GameObject stage1Canvas;
    public GameObject MSLpanel;
    public GameObject gunPanel;
    public GameObject RgPanel;
    public GameObject stage2Canvas;
    public PlaneShooting playerShooting;
    public TargetingManager playerMSL;
    public PlaneRailGunLaunch PlaneRailGunLaunch;
    public PostProcessVolume volume;
    public float glitchIntensity = 1f;
    public float d_effectTime = 1f;
    public float e_effectTime = 1f;
    private RLProGlitch3 glitch;

    void Start()
    {
        volume.profile.TryGetSettings(out glitch);
    }

    public void disableMSL()
    {
        StartCoroutine(d_Panel(MSLpanel));
    }
    public void enableMSL()
    {
        StartCoroutine(e_Panel(MSLpanel));
        playerMSL.enabled = true;
    }

    public void disableGun()
    {
        StartCoroutine(d_Panel(gunPanel));
    }
    public void enableGun()
    {
        StartCoroutine(e_Panel(gunPanel));
        //playerShooting.enabled = true;
    }

    public void disableRG()
    {
        StartCoroutine(d_Panel(RgPanel));
    }
    public void enableRG()
    {
        StartCoroutine(e_Panel(RgPanel));
        PlaneRailGunLaunch.enabled = true;
    }

    public void disableStage1Canvas()
    {
        StartCoroutine(d_Panel(stage1Canvas));
    }
    public void disableStage2Canvas()
    {
        StartCoroutine(d_Panel(stage2Canvas));
    }

    public void enableStage1Canvas()
    {
        StartCoroutine(e_Panel(stage1Canvas));
    }

    public void enableStage2Canvas()
    {
        StartCoroutine(e_Panel(stage2Canvas));
    }

    public void resetHUD()
    {
        stage2Canvas.SetActive(false);
        stage1Canvas.SetActive(true);
        MSLpanel.SetActive(true);
        gunPanel.SetActive(true);
        playerMSL.enabled = true;
        //playerShooting.enabled = true;
    }
    IEnumerator d_Panel(GameObject panel)
    {
        glitch.active = true;
        float timer = 0;
        glitch.speed.value = glitchIntensity;
        while (timer <= d_effectTime)
        {
            float interval1 = Random.Range(0.01f, 0.2f);
            float interval2 = Random.Range(0.2f, 0.7f);
            yield return new WaitForSeconds(interval2);
            panel.SetActive(false);
            yield return new WaitForSeconds(interval1);
            panel.SetActive(true);
            timer += interval1 + interval2;
        }
        panel.SetActive(false);
        glitch.speed.value = 0;
        glitch.active = false;
    }

    IEnumerator e_Panel(GameObject panel)
    {
        glitch.active = true;
        float timer = 0;
        glitch.speed.value = glitchIntensity;
        while (timer <= e_effectTime)
        {
            float interval1 = Random.Range(0.01f, 0.2f);
            float interval2 = Random.Range(0.2f, 0.7f);
            yield return new WaitForSeconds(interval2);
            panel.SetActive(true);
            yield return new WaitForSeconds(interval1);
            panel.SetActive(false);
            timer += interval1 + interval2;
        }
        panel.SetActive(true);
        glitch.speed.value = 0;
        glitch.active = false;
    }
}
