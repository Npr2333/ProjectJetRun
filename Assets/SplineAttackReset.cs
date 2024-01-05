using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineAttackReset : MonoBehaviour
{
    public GameObject boomTrigger;
    private SplineAnimate splineAnimate;
    private BossController bossController;
    private float duration;
    private bool restarting = false;

    private void Start()
    {
        bossController = gameObject.GetComponent<BossController>();
        splineAnimate = gameObject.GetComponent<SplineAnimate>();
        duration = splineAnimate.Duration;
    }

    private void Update()
    {
        if (splineAnimate.IsPlaying && !restarting)
        {
            StartCoroutine(restart());
        }
    }

    IEnumerator restart()
    {
        bossController.setState(2);
        boomTrigger.SetActive(true);
        restarting = true;
        yield return new WaitForSeconds(duration);
        bossController.setState(1);
        splineAnimate.Restart(false);
        restarting = false;
        boomTrigger.SetActive(false);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
