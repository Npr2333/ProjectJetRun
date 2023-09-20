using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public enum GameState
    {
        GameStart,
        Stage1,
        Transition1,
        Stage2,
        Transition2,
        Stage3,
    }

    public GameState currentState;

    // Start is called before the first frame update
    public void Start()
    {
        currentState = GameState.GameStart;    
    }

    // Update is called once per frame
    public void Update()
    {
        switch (currentState)
        {
            case GameState.GameStart:
            // Start the game
            GameStart();
            break;

            case GameState.Stage1:
            // Handle stage1 
            Stage1();
            break;

            case GameState.Transition1:
            // Handle transition1
            Transition1();
            break;

            case GameState.Stage2:
            // Handle stage2
            Stage2();
            break;

            case GameState.Transition2:
            // Handle transition2
            Transistion2();
            break;

            case GameState.Stage3:
            // Handle stage3
            break;
        }
    }

    public void GameStart()
    {
        // Code to start the game
    }

    public void Stage1()
    {
        // Code to handle stage 1
    }

    public void Transition1()
    {
        // Code to handle transition
    }

    public void Stage2()
    {
        // Code to handle stage 2
    }

    public void Transistion2()
    {
        // Code to handle transistion 2
    }

    public void ToStage1()
    {
        currentState = GameState.Stage1;
    }

    public void ToTransition1()
    {
        currentState = GameState.Transition1;
    }

    public void ToStage2()
    {
        currentState = GameState.Stage2;
    }

    public void ToTransisiton2()
    {
        currentState = GameState.Transition2;
    }

    public void ToStage3()
    {
        currentState = GameState.Stage3;
    }
}
