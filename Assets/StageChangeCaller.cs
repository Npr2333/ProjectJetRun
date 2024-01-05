using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChangeCaller : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
    }

    public void toStage1()
    {
        gameManager.SetCurrentState(GameManager.GameState.Stage1);
    }

    public void toStage2()
    {
        gameManager.SetCurrentState(GameManager.GameState.Stage2);
    }

    public void toTransition2()
    {
        gameManager.SetCurrentState(GameManager.GameState.Transition2);
    }

    public void toStage3()
    {
        gameManager.SetCurrentState(GameManager.GameState.Stage3);
    }

    public void toTransition3()
    {
        gameManager.SetCurrentState(GameManager.GameState.Transition3);
    }

    public void toCountScore()
    {
        gameManager.SetCurrentState(GameManager.GameState.ScoreCount);
    }
    public void toGameOver()
    {
        gameManager.SetCurrentState(GameManager.GameState.GameOver);
    }
}
