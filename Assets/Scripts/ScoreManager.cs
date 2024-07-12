using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int maxScore;

    public int currentScore = 0;
    void Start()
    {
        // ResetScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetScore()
    {
        currentScore = 0;
    }

    public void AddScore(int value)
    {
        currentScore += value;
        Debug.Log("Score: " + currentScore);
        // UpdateScoreBar();
    }
}
