using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public static GameObject MainGameObject;
    public static CoroutineHandler coroutineHandler;

    public Material DaySkybox;
    public Material NightSkybox;

    DayNightCycle dayNightCycle = new DayNightCycle();


    void  Awake() {
        MainGameObject = gameObject;
        coroutineHandler = MainGameObject.GetComponent<CoroutineHandler>();

        dayNightCycle.changed += DayNightToggle;
        dayNightCycle.Start();
    }

    void DayNightToggle() {
        if (dayNightCycle.dayNight == DayNightCycle.DayNight.Day) {
            RenderSettings.skybox = DaySkybox;
        } else {
            RenderSettings.skybox = NightSkybox;
        }
    }
}
