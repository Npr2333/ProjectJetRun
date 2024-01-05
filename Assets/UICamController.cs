using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamController : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = this.GetComponent<Camera>();
    }
    public void setDepth(int num)
    {
        cam.depth = num;
    }
}
