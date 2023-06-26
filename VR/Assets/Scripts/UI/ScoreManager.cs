using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public TextMeshPro scoreText;

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = ("Score: " + score.ToString());
    }

}
