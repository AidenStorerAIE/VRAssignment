using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    public List<Score> scores;
    public int maxScoreCount;

    private void Start()
    {
        for (int i = 0; i < maxScoreCount; i++)
            scores[i].score = 0;
    }

    public void AddScore(int score)
    {
        for(int i = 0; i < scores.Count; i++)
        {
            //first score
            if (scores[i].score == 0)
            {
                scores[i].score = score;
                scores[i].ChangeScore();
                return;
            }
            if (score > scores[i].score)
            {
                if (scores[i + 1] && score > scores[i + 1].score)
                {
                    scores[i].score = score; 
                    scores[i].ChangeScore();
                    Sort();
                    return;
                }
                else if (!scores[i + 1])
                {
                    scores[i].score = score;
                    scores[i].ChangeScore();
                    Sort();
                    return;
                }
            }
        }
    }

    void Sort()
    {
        for(int i = 0; i < scores.Count - 1; i++)
        {
            for(int j = 0; j < scores.Count - 1 - i; j++)
            {
                if (scores[j].score < scores[j + 1].score)
                {
                    Score tempScore = scores[j];
                    scores[j] = scores[j + 1];
                    scores[j + 1 ] = tempScore;
                }
            }
        }
    }
}
