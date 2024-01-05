using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartMenu : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void StartGame()
    {
        gameManager.StartGame();
    }
}
