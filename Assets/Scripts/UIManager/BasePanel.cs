using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public UIPanelType panelType;
    
    public virtual void OnEnter()
    {
		gameObject.SetActive(true);
    }

    public virtual void OnPause()
    {

    }

    public virtual void OnExit()
    {
		gameObject.SetActive(false);
    }
}
