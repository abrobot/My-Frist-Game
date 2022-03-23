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
    static public GameObject player;

    static int GroupMaxDifficulty = 500;
    static int GroupMinDifficulty = 300;

    void Awake() {
        player = gameObject;

        DifficultyPoints = 500;
        StartCoroutine(DifficultyPointsGenerator());
        StartCoroutine(Activate(player));

    }

     public static IEnumerator  Activate(GameObject player) {
        while (true) {
            for (int i = 0; DifficultyPoints >= GroupMinDifficulty; i++) {
                EnemyGroup enemyGroup = new EnemyGroup();
                enemyGroup.GenerateGroup();
            }
            yield return new WaitForSeconds(1);
        }
    }


    
    public static IEnumerator DifficultyPointsGenerator() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(10,20));
            DifficultyPoints += Random.Range(400, 600);
            //DifficultyPoints += Random.Range(0, 0);
        }
    }



    
    class EnemyGroup {

        int numberOfEnemys;
        int PointAvailable;
        
        Vector3 position;

        public void GenerateGroup() {
            PointAvailable = CalculateAvailableDifficultyPoints();
            position = FindAcceptablePositionForGroup();
            for (int i = 0; PointAvailable >= 100; i++) {
                int pointsConsumed = Enemy.SpawnRandomEnemy(position);
                DifficultyPoints -= pointsConsumed;
            }
        }


        static int CalculateAvailableDifficultyPoints() {
            bool enoughForGroup = (DifficultyPoints >= GroupMinDifficulty);
            if (!enoughForGroup) {
                return 0;
            }

            bool useMax = (DifficultyPoints >= GroupMaxDifficulty);
            int PointAvailable = 0;

            if (useMax) {
                PointAvailable = GroupMaxDifficulty;
            } else {
                PointAvailable = DifficultyPoints;
            }

            return MyMathFuctions.RoundNum(Random.Range(GroupMinDifficulty, PointAvailable), 100);
        }


        static Vector3 FindAcceptablePositionForGroup() {
            float positionX = Random.Range(player.transform.position.x - (160 / 2), player.transform.position.x + (160 / 2));
            float positionZ = Random.Range(player.transform.position.z - (160 / 2), player.transform.position.z + (160 / 2));

            return new Vector3(positionX, 20, positionZ);
        }
    }
}



