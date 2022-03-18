using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript
{
    public static GameObject MainGameObject = GameObject.Find("Game");
    public static CoroutineHandler coroutineHandler = MainGameObject.GetComponent<CoroutineHandler>();

}
