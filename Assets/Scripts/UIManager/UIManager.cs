using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Codes.Framework;

public enum UIPanelType
{
	MainHud,
	StartMenu,
	ScoreMenu,
	OverMenu,
}

public class UIManager : MonoSingleton<UIManager>
{

	private Transform canvasTransform;
	private Transform CanvasTransform

	{
		get
		{
			if (canvasTransform == null)
			{
				canvasTransform = GameObject.Find("MenuCanvas").transform;
			}
			return canvasTransform;
		}
	}
	private Dictionary<UIPanelType, BasePanel> panelDict;//保存所有实例化面板的游戏物体身上的BasePanel组件
	private List<BasePanel> panelStack;

	//入栈：显示出某个页面
	public void PushPanel(UIPanelType panelType)
	{
		if (panelStack == null)
		{
			panelStack = new List<BasePanel>();
		}

		//判断栈里面是否有页面
		if (panelStack.Count > 0)
		{
			int topIndex = panelStack.Count - 1;
			BasePanel topPanel = panelStack[topIndex];
			topPanel.OnPause();
		}
		BasePanel panel = GetPanel(panelType);
		panel.panelType = panelType;
		panel.OnEnter();
		panelStack.Add(panel);
	}

	//出栈：移除某个页面
	public void PopPanel()
	{
		int topIndex = panelStack.Count - 1;
		var panel = panelStack[topIndex];
		panel.OnExit();
		panelStack.RemoveAt(topIndex);		
	}

	public void RemovePanel(UIPanelType panelType)
    {
		foreach(var panel in panelStack)
        {
			if(panel.panelType == panelType)
            {
				panel.OnExit();
				panelStack.Remove(panel);
				break;
            }
        }
    }


	public BasePanel GetPanel(UIPanelType panelType)
	{
		if (panelDict == null)
		{
			panelDict = new Dictionary<UIPanelType, BasePanel>();
		}

		BasePanel panel = null;
		panelDict.TryGetValue(panelType, out panel);

		if (panel == null)
		{
			//如果找不到，就找这个面板的prefab路径，然后根据prefab去实例化面板

			string path = Path.Combine("UIPanels", panelType.ToString());
			GameObject instPanel = Instantiate(Resources.Load(path)) as GameObject;
			instPanel.transform.SetParent(CanvasTransform, false);
			panelDict[panelType] = instPanel.GetComponent<BasePanel>();
			return instPanel.GetComponent<BasePanel>();
		}
		else
		{
			return panel;
		}
	}

}
