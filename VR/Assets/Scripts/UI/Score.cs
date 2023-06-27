using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score;
    TextMeshPro scoreText;

    private void Start()
    {
        scoreText = GetComponent<TextMeshPro>();    
    }

    public void ChangeScore()
    {
        scoreText.text = ("Score: " + score);
    }


}
