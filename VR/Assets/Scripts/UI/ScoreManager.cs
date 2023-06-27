using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public TextMeshPro scoreText;
    float startTime;
    public Timer timer;
    public HighScoreManager highscoreManager;

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = ("Score: " + score.ToString());
    }

    public void StartTimer()
    {
        if(score != 0)
        {

            score = 0;
        }

        startTime = Time.time;
    }

    public void Done()
    {
        if((int)(startTime - Time.time) < Time.time)
        score += (int)(startTime - Time.time);//+ plus amount 
        UpdateUI();
        highscoreManager.AddScore(score);
        timer.Stop();
    }
}
