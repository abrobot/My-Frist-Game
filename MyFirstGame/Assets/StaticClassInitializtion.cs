using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticClassInitializtion : MonoBehaviour
{
    
    void Awake()
    {
        Difficulty.DifficultyConstructor(); 

    }


}
