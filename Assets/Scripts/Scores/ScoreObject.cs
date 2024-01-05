using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObject
{
    public string message;
    public float timer = 3f;
    public int score = 0;

    public ScoreObject(string message, float timer, int score)
    {
        this.message = message;
        this.timer = timer;
        this.score = score;
    }
}
