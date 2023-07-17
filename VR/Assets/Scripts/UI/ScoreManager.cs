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
    public TextMeshPro highScoreText;

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = (score.ToString());
    }

    void UpdateHighScoreUI()
    {
        highScoreText.text = (highscoreManager.scores[0].ToString());
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
        highscoreManager.AddScore(score);
        UpdateUI();
        UpdateHighScoreUI();
        timer.Stop();
    }
}
