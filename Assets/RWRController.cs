using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RWRController : MonoBehaviour
{
    public Transform plane;
    public RWRTrigger rwrTrigger;
    private Color restColor;
    public Color threatColor;
    public float detectDistance;
    public float closeClipDistance = 50f;
    public float resetTime;
    public int affectedSliders = 5;
    public float restSliderValue = 0f;
    public bool isUpdown = false;
    private List<GameObject> threatList = new List<GameObject>();
    public List<Slider> sliders = new List<Slider>();
    public List<timedSlider> t_sliders = new List<timedSlider>();
    private List<IEnumerator> myCoroutines;
    private bool[] indicators = new bool[128];
    private bool[] indicators2 = new bool[128];
    private float[] timeTracker = new float[128];
    private float degree = 0;

    public class timedSlider
    {
        public float trackTime;
        public bool indicator;
        public Slider slider;

        public timedSlider(Slider slider ,float trackTime, bool indicator)
        {
            this.slider = slider;
            this.trackTime = trackTime;
            this.indicator = indicator;
        }
    }
    private void Start()
    {
        foreach (Transform child in transform)
        {
            //sliders.Add(child.GetComponent<Slider>());
            t_sliders.Add(new timedSlider(child.GetComponent<Slider>(), 0, false));
        }
        restColor = t_sliders[0].slider.fillRect.GetComponent<Image>().color;
        myCoroutines = new List<IEnumerator>(128);
        //for (int i = 0; i < 128; i++)
        //{
        //    myCoroutines.Add(LerpBack(sliders[i], i));
        //}
    }
    private void Update()
    {
        threatList = rwrTrigger.GetObjectsInTrigger();

        //for (int i = 0; i < 128; i++)
        //{
        //    if (!indicators[i])
        //    {
        //        indicators[i] = true;
        //        myCoroutines[i] = LerpBack(sliders[i], i);
        //        StartCoroutine(myCoroutines[i]);
        //    }
        //}
        //LerpBack();
        //markthreats();
    }
    private void LateUpdate()
    {
        LerpBack();
        markthreats();
    }
    private void markthreats()
    {
        //foreach (GameObject threat in threatList)
        //{
        //    float distance = Vector3.Distance(plane.position, threat.transform.position);
        //    Vector3 line = threat.transform.position - plane.position;
        //    line.Normalize();
        //    float degree = Mathf.Atan2(line.y , line.x) * Mathf.Rad2Deg;
        //    //Debug.Log(degree);
        //    if (degree < 0)
        //    {
        //        degree += 360;
        //    }
        //    else if (degree > 360)
        //    {
        //        degree -= 360;
        //    }
        //    int index  = (int)(127 * degree / 360);
        //    int upTo = (int)Mathf.Ceil((affectedSliders / 2));
        //    for (int i = 0; i < upTo; i++)
        //    {
        //        int upIndex = (index + i) % 127;
        //        int lowIndex = (index - i);
        //        if (lowIndex < 0)
        //        {
        //            lowIndex += 127;
        //        }
        //        float percent = (upTo - i) / (float)upTo;
        //        StopCoroutine(myCoroutines[upIndex]);
        //        indicators[upIndex] = false;
        //        Image fillUpImage = sliders[upIndex].fillRect.GetComponent<Image>();
        //        fillUpImage.color = Color.Lerp(restColor, threatColor, (1 - (distance / detectDistance)) * percent);
        //        StopCoroutine(myCoroutines[lowIndex]);
        //        indicators[lowIndex] = false;
        //        Image fillLowImage = sliders[lowIndex].fillRect.GetComponent<Image>();
        //        fillLowImage.color = Color.Lerp(restColor, threatColor, (1 - (distance / detectDistance)) * percent);
        //    }
        //    StopCoroutine(myCoroutines[index]);
        //    indicators[index] = false;
        //    Image fillImage = sliders[index].fillRect.GetComponent<Image>();
        //    fillImage.color = Color.Lerp(restColor, threatColor, 1 - (distance / detectDistance));
        //}
        if (gameObject)
        {
            foreach (GameObject threat in threatList)
            {
                if (threat && !isUpdown)
                {
                    if (threat.tag == "SplineBoss")
                    {
                        continue;
                    }
                    //Calculate the degree of line to the threat
                    float distance = Vector3.Distance(plane.position, threat.transform.position);
                    Vector3 line = threat.transform.position - plane.position;
                    line.Normalize();
                    degree = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
                    if (degree < 0)
                    {
                        degree += 360;
                    }
                    else if (degree > 360)
                    {
                        degree -= 360;
                    }
                    //Calculate which index to change
                    int index = (int)((t_sliders.Count - 1) * degree / 360);
                    int upTo = (int)Mathf.Ceil((affectedSliders / 2));
                    float distancePercent = Mathf.Max(0, distance - closeClipDistance) / detectDistance;
                    //Change the surrounding sliders
                    for (int i = 0; i < upTo; i++)
                    {
                        //Calculate the upper and lower indexes
                        int upIndex = (index + i) % (t_sliders.Count);
                        int lowIndex = (index - i);
                        if (lowIndex < 0)
                        {
                            lowIndex += (t_sliders.Count);
                        }
                        float percent = (upTo - i) / (float)upTo;
                        //Change the upper sliders
                        t_sliders[upIndex].indicator = true;
                        t_sliders[upIndex].trackTime = 0;
                        Image fillUpImage = t_sliders[upIndex].slider.fillRect.GetComponent<Image>();
                        fillUpImage.color = Color.Lerp(restColor, threatColor, (1 - distancePercent) * percent);
                        t_sliders[upIndex].slider.value = Mathf.Lerp(restSliderValue, 1, (1 - distancePercent) * percent);

                        //Change the lower sliders
                        t_sliders[lowIndex].indicator = true;
                        t_sliders[lowIndex].trackTime = 0;
                        Image fillLowImage = t_sliders[lowIndex].slider.fillRect.GetComponent<Image>();
                        fillLowImage.color = Color.Lerp(restColor, threatColor, (1 - distancePercent) * percent);
                        t_sliders[lowIndex].slider.value = Mathf.Lerp(restSliderValue, 1, (1 - distancePercent) * percent);
                    }

                    //Change the main slider
                    t_sliders[index].indicator = true;
                    t_sliders[index].trackTime = 0;
                    Image fillImage = t_sliders[index].slider.fillRect.GetComponent<Image>();
                    fillImage.color = Color.Lerp(restColor, threatColor, (1 - distancePercent));
                    t_sliders[index].slider.value = Mathf.Lerp(restSliderValue, 1, (1 - distancePercent));                    
                }else if (threat && isUpdown)
                {
                    //Calculate the degree of line to the threat
                    float distance = Vector3.Distance(plane.position, threat.transform.position);
                    Vector3 line = threat.transform.position - plane.position;
                    line.Normalize();
                    degree = Mathf.Atan2(line.z, line.x) * Mathf.Rad2Deg;
                    if (degree < 0)
                    {
                        degree += 360;
                    }
                    else if (degree > 360)
                    {
                        degree -= 360;
                    }
                    //Calculate which index to change
                    int index = (int)((t_sliders.Count - 1) * degree / 360);
                    int upTo = (int)Mathf.Ceil((affectedSliders / 2));
                    float distancePercent = Mathf.Max(0, distance - closeClipDistance) / detectDistance;
                    //Change the surrounding sliders
                    for (int i = 0; i < upTo; i++)
                    {
                        //Calculate the upper and lower indexes
                        int upIndex = (index + i) % (t_sliders.Count);
                        int lowIndex = (index - i);
                        if (lowIndex < 0)
                        {
                            lowIndex += (t_sliders.Count);
                        }
                        float percent = (upTo - i) / (float)upTo;
                        //Change the upper sliders
                        t_sliders[upIndex].indicator = true;
                        t_sliders[upIndex].trackTime = 0;
                        Image fillUpImage = t_sliders[upIndex].slider.fillRect.GetComponent<Image>();
                        fillUpImage.color = Color.Lerp(restColor, threatColor, (1 - distancePercent) * percent);
                        t_sliders[upIndex].slider.value = Mathf.Lerp(restSliderValue, 1, (1 - distancePercent) * percent);

                        //Change the lower sliders
                        t_sliders[lowIndex].indicator = true;
                        t_sliders[lowIndex].trackTime = 0;
                        Image fillLowImage = t_sliders[lowIndex].slider.fillRect.GetComponent<Image>();
                        fillLowImage.color = Color.Lerp(restColor, threatColor, (1 - distancePercent) * percent);
                        t_sliders[lowIndex].slider.value = Mathf.Lerp(restSliderValue, 1, (1 - distancePercent) * percent);
                    }

                    //Change the main slider
                    t_sliders[index].indicator = true;
                    t_sliders[index].trackTime = 0;
                    Image fillImage = t_sliders[index].slider.fillRect.GetComponent<Image>();
                    fillImage.color = Color.Lerp(restColor, threatColor, (1 - distancePercent));
                    t_sliders[index].slider.value = Mathf.Lerp(restSliderValue, 1, (1 - distancePercent));
                }
            }
        }
    }

    private void LerpBack()
    {
        for (int i = 0; i < t_sliders.Count; i++)
        {
            //if (indicators2[i])
            //{
            //    if (timeTracker[i] >= resetTime)
            //    {
            //        indicators2[i] = false;
            //        continue;
            //    }
            //    Color _curr = sliders[i].fillRect.GetComponent<Image>().color;
            //    Image fillLowImage = sliders[i].fillRect.GetComponent<Image>();
            //    fillLowImage.color = Color.Lerp(_curr, restColor, timeTracker[i] / resetTime);
            //    sliders[i].value = Mathf.Lerp(sliders[i].value, restSliderValue, timeTracker[i] / resetTime);
            //    timeTracker[i] += Time.deltaTime; 
            //}
            if (t_sliders[i].indicator)
            {
                if (t_sliders[i].trackTime >= resetTime)
                {
                    t_sliders[i].indicator = false;
                    continue;
                }
                Color _curr = t_sliders[i].slider.fillRect.GetComponent<Image>().color;
                Image fillLowImage = t_sliders[i].slider.fillRect.GetComponent<Image>();
                fillLowImage.color = Color.Lerp(_curr, restColor, t_sliders[i].trackTime / resetTime);
                t_sliders[i].slider.value = Mathf.Lerp(t_sliders[i].slider.value, restSliderValue, t_sliders[i].trackTime / resetTime);
                t_sliders[i].trackTime += Time.deltaTime;
            }
        }
    }

    //IEnumerator LerpBack(Slider slider, int index)
    //{
        
    //    float _timer = 0;
    //    Color _curr = slider.fillRect.GetComponent<Image>().color;
    //    while (_timer < resetTime)
    //    {
    //        Image fillImage = slider.fillRect.GetComponent<Image>();
    //        fillImage.color = Color.Lerp(_curr, restColor, _timer / resetTime);
    //        _timer += Time.deltaTime;

    //        yield return null;
    //    }
    //    indicators[index] = false;
    //}
}
