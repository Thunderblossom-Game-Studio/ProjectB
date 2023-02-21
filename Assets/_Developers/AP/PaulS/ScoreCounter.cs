using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI ScoreText;
    public int score = 0;
    public int maxScore;
    
    public GameObject objectiveTimer;
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
        ScoreText.text = "PP (Package Points): " + score;
    }

    void Update()
    {
        UpdateScore();

        if (score == maxScore)
        {
            Score.SetActive(false);
            WinText.SetActive(true);
            objectiveTimer.SetActive(false);
        }

        //Debug/Testing below

        if (Input.GetKeyDown(KeyCode.K))
        {
            AddScore(1);
        }
    }


}
