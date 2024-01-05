using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaneHUD : MonoBehaviour
{   
    public Canvas worldSpaceCanvas;
    public RectTransform center;
    public LayerMask layer;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI healthText2;
    public Transform planeModel;
    public PlaneHealth planeHealth;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        setCenter();
        setHealth();
    }

    private void setCenter()
    {
        Ray ray = new Ray(planeModel.transform.position, planeModel.transform.forward); // Create a ray from the object
        Debug.DrawRay(ray.origin, ray.direction * 5000, Color.red);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo,1000,layer))
        {
            //Debug.Log("Hit");
            if (hitInfo.collider.gameObject == worldSpaceCanvas.gameObject)
            {
                //Debug.Log("Hit");
                // Convert the hit point to a position relative to the canvas
                Vector2 canvasPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    worldSpaceCanvas.GetComponent<RectTransform>(),
                    hitInfo.point,
                    null,
                    out canvasPos
                );

                // Move the UI element
                center.anchoredPosition = canvasPos;
                Quaternion modelRotation = planeModel.rotation;
                Quaternion targetRotation = Quaternion.Euler(0, 0, modelRotation.eulerAngles.z);
                center.rotation = targetRotation;
            }
        }
    }

    private void setHealth()
    {
        if (healthText)
        {
            healthText.text = (planeHealth.getHealth()).ToString();
            healthText2.text = (planeHealth.getHealth()).ToString();
        }
    }
}
