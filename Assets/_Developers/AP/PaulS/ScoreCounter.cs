using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{

    public Text ScoreText;
    public int score = 0;
    public int maxScore;

    public GameObject Score;
    public GameObject WinText;

    void Start()
    {
        score = 0;
    }

    public void AddScore(int newScore)
    {
        score += newScore;
    }
    
    public void UpdateScore()
    {
        ScoreText.text = "Score: " + score;
    }

    void Update()
    {
        UpdateScore();

        if (score == maxScore)
        {
            Score.SetActive(false);
            WinText.SetActive(true);
        }

        //Debug/Testing below

        if (Input.GetKeyDown(KeyCode.K))
        {
            AddScore(1);
        }
    }


}
