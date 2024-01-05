using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverMenuPanel : BasePanel
{
    public void OnStartButtonClicked()
    {
        Application.Quit();
    }
}
