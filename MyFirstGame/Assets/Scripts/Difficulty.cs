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
    public GameObject player;

    void Awake() {
        DifficultyPoints = 500;
        StartCoroutine(DifficultyPointsGenerator());
        StartCoroutine(Activate(player));

    }

     public static IEnumerator  Activate(GameObject player) {
        while (true) {
            for (int i = 0; DifficultyPoints >= 100; i++) {
                int pointsConsumed = Enemy.SpawnRandomEnemy(player);
                DifficultyPoints -= pointsConsumed;
            }
            yield return new WaitForSeconds(1);
        }
    }


    
    public static IEnumerator DifficultyPointsGenerator() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(10,20));
            //DifficultyPoints += Random.Range(400, 600);
            DifficultyPoints += Random.Range(0, 0);
        }
    }
}



