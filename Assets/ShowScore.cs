using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowScore : MonoBehaviour
{
    public GameObject S ;
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public TextMeshProUGUI scoreNumber;
    public int S_threshold;
    public int A_threshold;
    public int B_threshold;

    private void Awake()
    {
        S.SetActive(false);
        A.SetActive(false);
        B.SetActive(false);
        C.SetActive(false);
    }
    public void showScore(int score)
    {
        scoreNumber.text = score.ToString();
        if (score > S_threshold)
        {
            S.SetActive(true);
            return;
        }
        if (score > A_threshold)
        {
            A.SetActive(true);
            return;
        }
        if (score > B_threshold)
        {
            B.SetActive(true);
            return;
        }
        C.SetActive(true);
        return;
    }

    public void resetScore()
    {
        S.SetActive(false);
        A.SetActive(false);
        B.SetActive(false);
        C.SetActive(false);
    }
}
