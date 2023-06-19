using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float minutes;
    public float seconds;
    bool active;
    public TextMeshPro timerText;
    public int secondsStringLength;

    private void Start()
    {
        UpdateUI();
    }

    public void Activate()
    {
        active = true;
    }

    public void Stop()
    {
        active = false;
    }

    private void Update()
    {
        //debugging
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Activate();
        }

        if (!active)
            return;

        seconds -= Time.deltaTime;

        if(seconds < 0)
        {
            if(minutes > 0)
            {
                minutes--;
                seconds = 60;
            }
            else
            {
                Stop();
                seconds = 0;
            }
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        string secondsString = seconds.ToString().Length > secondsStringLength ? 
            seconds.ToString().Substring(0, secondsStringLength) : seconds.ToString();
        timerText.text = ("Time: " + minutes.ToString() + ":" + secondsString);
    }
}
