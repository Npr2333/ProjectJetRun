using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreQueue: MonoBehaviour
{
    public TextMeshProUGUI[] textList;
    public TextMeshProUGUI scoreSection;
    private int scoreNumber;
    private LinkedList<ScoreObject> scoreList = new LinkedList<ScoreObject>();
    private List<ScoreObject> objectsToRemove = new List<ScoreObject>();
    
    public void AddToList(ScoreObject score)
    {
        if (scoreList.Count + 1 > textList.Length)
        {
            scoreList.Remove(scoreList.Last);
            scoreList.AddFirst(score);
            scoreNumber += score.score;
            scoreSection.text = scoreNumber.ToString();
            return;
        }
        scoreList.AddFirst(score);
        scoreNumber += score.score;
        scoreSection.text = scoreNumber.ToString();
    }

    private void Update()
    {
        checkTimer();
        removeObjects();
        updateText();
    }

    private void updateText()
    {
        int index = 0;
        foreach (ScoreObject score in scoreList)
        {
            textList[index].text = score.message;
            index++;
        }

        for (int i = index; i < textList.Length; i++)
        {
            textList[i].text = "";
        }

    }
    private void checkTimer()
    {
        objectsToRemove.Clear();
        for (LinkedListNode<ScoreObject> node = scoreList.First; node != null; node = node.Next)
        {
            node.Value.timer -= Time.deltaTime;
            if (node.Value.timer <= 0)
            {
                objectsToRemove.Add(node.Value);
            }
        }
    }

    private void removeObjects()
    {
        foreach (ScoreObject element in objectsToRemove)
        {
            scoreList.Remove(element);
        }
    }

    public int getScore()
    {
        return scoreNumber;
    }

    public void resetStatus()
    {
        scoreNumber = 0;
        scoreSection.text = scoreNumber.ToString();
        scoreList.Clear();
        objectsToRemove.Clear();
        for (int i = 0; i < textList.Count(); i++)
        {
            textList[i].text = "";
        }
    }
}
