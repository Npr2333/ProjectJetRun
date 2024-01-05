using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineReset : MonoBehaviour
{
    public SplineAnimate splineAnimate;
    private float duration;
    private bool restarting = false;

    private void Start()
    {
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
        restarting = true;
        yield return new WaitForSeconds(duration);
        splineAnimate.Restart(false);
        restarting = false;
    }
}
