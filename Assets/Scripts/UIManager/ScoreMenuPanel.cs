using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMenuPanel : BasePanel
{
    public GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void OnContinueClicked()
    {
        gameManager.SetCurrentState(GameManager.GameState.GameOver);
    }
}
