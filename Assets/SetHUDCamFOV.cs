using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHUDCamFOV : MonoBehaviour
{
    public Camera hudCamera;

    private void Update()
    {
        if (gameObject.GetComponent<CinemachineVirtualCamera>().Priority > 2)
        {
            hudCamera.fieldOfView = gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        }
    }

}
