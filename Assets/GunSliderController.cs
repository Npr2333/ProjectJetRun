using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class GunSliderController : MonoBehaviour
{
    public GameObject indicator;

    public void indicateFire(bool indicate)
    {
        if (indicate)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }
    }
}
