using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuPanel : BasePanel
{
    private GameManager gameManager;
    //public UIPanelType StartMenu;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonClicked()
    {
        gameManager.StartGame();
    }
}
