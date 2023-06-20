using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float startMinutes, startSeconds;
    float minutes, seconds;
    bool active;
    public TextMeshPro timerText;
    public int secondsStringLength;
    public TargetManager targetManager;

    private void Start()
    {
        Reset();
        UpdateUI();
    }

    public void Activate()
    {
        active = true;
    }

    public void Stop()
    {
        active = false;
        //for (int i = 0; i < targetManager.targetCount; i++)
        //{
        //    targetManager.DropTarget(targetManager.gameObjects[i], 0); //0 score
        //}
        targetManager.Stop();
    }

    public void Reset()
    {
        minutes = startMinutes;
        seconds = startSeconds;
        UpdateUI();
        Stop();
    }

    private void Update()
    {
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
