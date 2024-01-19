using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RgSliderController : MonoBehaviour
{
    public Slider mslSlider;
    private float resetTime;
    private float _timer = 0f;
    private bool isSliding = false;

    private void Update()
    {
        if (isSliding)
        {
            _timer += Time.deltaTime;
        }
    }
    public void resetSlider(float targetTime)
    {
        isSliding = true;
        _timer = 0;
        mslSlider.value = 0;
        resetTime = targetTime;
        StartCoroutine(slide());
    }

    IEnumerator slide()
    {
        float _curr = mslSlider.value;
        float _initial = mslSlider.value;

        while (_curr != resetTime)
        {
            _curr = Mathf.Lerp(_initial, 1, _timer / resetTime);

            mslSlider.value = _curr;

            yield return null;
        }
        isSliding = false;
    }
}
