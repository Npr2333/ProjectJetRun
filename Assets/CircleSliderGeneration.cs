using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSliderGeneration : MonoBehaviour
{
    public GameObject sliderPrefab; // Assign your UI slider prefab
    public int numberOfSliders = 8;
    public float radius = 100f;
    public Canvas canvas;
    public Transform parent;

    public void GenerateSliders()
    {
        for (int i = 0; i < numberOfSliders; i++)
        {
            // Calculate angle around the circle
            float angle = i * Mathf.PI * 2f / numberOfSliders;
            Vector2 position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            // Instantiate the slider as a child of the canvas
            GameObject sliderGO = Instantiate(sliderPrefab, canvas.transform, false);
            RectTransform sliderRect = sliderGO.GetComponent<RectTransform>();

            // Set the position of the slider
            sliderRect.anchoredPosition = position;

            // Rotate the slider to be tangential to the circle
            sliderRect.localEulerAngles = new Vector3(0, 0, (float)(angle * (180/Math.PI)));

            sliderRect.transform.parent = parent;
        }
    }
}
