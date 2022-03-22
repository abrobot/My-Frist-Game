using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameTimer
{

    public TimeSpan currentTimeSpan = new TimeSpan();
   [NonSerializedAttribute] public float currentTime = 0f;
    bool timerRunning = false;

    public TextMeshProUGUI TextElement;

    public void StartTimer(){
        timerRunning = true;
        GameScript.coroutineHandler.callCoroutine(StartUpdateLoop());
    }

    public void Stop() {
        timerRunning = false;
    }

    IEnumerator StartUpdateLoop() {
        while (timerRunning) {
            currentTime += Time.deltaTime;
            currentTimeSpan = TimeSpan.FromSeconds(currentTime);
            if (TextElement) {
                TextElement.text = currentTimeSpan.ToString("mm':'ss'.'ff");
            } 

            yield return null;
        }
    }
}
