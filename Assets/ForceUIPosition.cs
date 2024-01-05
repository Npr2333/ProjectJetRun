using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceUIPosition : MonoBehaviour
{
    public Vector2 position;
    private RectTransform element;

    private void Start()
    {
        element = gameObject.GetComponent<RectTransform>();
    }
    private void Update()
    {
        element.anchoredPosition = position;
    }
}
