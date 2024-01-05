using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    public Vector2 xAngleRange = new Vector2(0, 360);
    public Vector2 yAngleRange = new Vector2(0, 360);
    public Vector2 zAngleRange = new Vector2(0, 360);

    public void rotate()
    {
        foreach (Transform child in transform)
        {
            RandomlyRotateChild(child);
        }
    }

    private void RandomlyRotateChild(Transform child)
    {
        float xAngle = Random.Range(xAngleRange.x, xAngleRange.y);
        float yAngle = Random.Range(yAngleRange.x, yAngleRange.y);
        float zAngle = Random.Range(zAngleRange.x, zAngleRange.y);

        child.localRotation = Quaternion.Euler(xAngle, yAngle, zAngle);
    }
}
