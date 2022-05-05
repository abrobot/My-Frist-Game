using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle
{
    public Status status;
    public DayNight dayNight;
    
    public int cycleLangthSeconds = 60;
    public int SecondsIntoCycle = 0;

    Coroutine cycleCoroutine;

    public delegate void OnDayNightChange();
    public event OnDayNightChange changed;

    public void Start() {
        cycleCoroutine = Game.coroutineHandler.StartCoroutine(StartCycle());
    }

    public void Pause() {
        status = Status.Paused;
    }

    public void Kill() {
        Game.coroutineHandler.StopCoroutine(cycleCoroutine);
        SecondsIntoCycle = 0;
    }

    void ToggleDayNight() {
        if (dayNight == DayNight.Day) {
            dayNight = DayNight.Night;
            changed.Invoke();
        } else {
            dayNight = DayNight.Day;
            changed.Invoke();
        } 
    }

    IEnumerator StartCycle() {
        status = Status.Running;
        dayNight = DayNight.Day;
        changed.Invoke();
        
        while (status == Status.Running | status == Status.Paused) {
            
            while( status == Status.Paused) {
                yield return new WaitForSeconds(1);
            }

            yield return new WaitForSeconds(1);;
            
            SecondsIntoCycle = SecondsIntoCycle + 1;

            if (SecondsIntoCycle == cycleLangthSeconds / 2) {
                ToggleDayNight();
            } else if (SecondsIntoCycle >= cycleLangthSeconds) {
                SecondsIntoCycle = 0;
                ToggleDayNight();
            }

        }
    }



    public enum Status {Running, Paused, Done}; 
    public enum DayNight {Day, Night}; 
}
