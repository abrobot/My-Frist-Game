using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using Unity.AI;
using Unity.AI.Navigation;


public class Difficulty : MonoBehaviour
{
    static int DifficultyPoints;


    void Awake() {
        DifficultyPoints = 200;
        print(34);
        ActivateDifficulty.Activate();
    }


    static class ActivateDifficulty 
    {
        static Dictionary<string, System.Func<int>> ConsumeDifficultyOptionsMethods = new Dictionary<string, System.Func<int>>();
        static List<string> ConsumeDifficultyOptionsMethodsKeys;


        static public void Activate() {
            print(13);
            ConsumeDifficultyOptionsMethods.Add("SpawnEnemy",  Enemy.SpawnRandomEnemy);
            ConsumeDifficultyOptionsMethodsKeys = new List<string>(ConsumeDifficultyOptionsMethods.Keys);

            for (int i = 0; DifficultyPoints > 0; i++) {
                int randomSelectionNumber = Random.Range(0, ConsumeDifficultyOptionsMethodsKeys.Count);
                string selection = ConsumeDifficultyOptionsMethodsKeys[randomSelectionNumber];
                int pointsConsumed = ConsumeDifficultyOptionsMethods[selection]();
                DifficultyPoints -= pointsConsumed;
            }
        }
    }
}



