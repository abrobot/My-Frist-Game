using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Difficulty 
{
    static int DifficultyPoints;
    //static float ActiveDifficultyLangthMinutes;

    public static void DifficultyConstructor() {
        SpawnEnemy.SpawnEnemyConstructor();
        CalculateDifficulty();



        ActivateDifficulty.ActivateDifficultyConstructor();
    }

    public static void CalculateDifficulty() {
        DifficultyPoints = 100;
        //ActiveDifficultyLangthMinutes = .5f;
    }




    public static class SpawnEnemy {
        
        static List<GameObject> enemySelection;
        static GameObject EnemysGameObject;

        public static void SpawnEnemyConstructor(){
            enemySelection = GetEnemyOptions();
            EnemysGameObject = GameObject.Find("BotsFolder");
        }

        public static  int SpawnRandomEnemy(){
            GameObject enemy = selectRandomEnemyType();
            GameObject newEnemy = Object.Instantiate(enemy, new Vector3(30, 0, 0), Quaternion.identity, EnemysGameObject.transform);
            return 100;
        }

        static GameObject selectRandomEnemyType(){
                int randomSelectionNumber = Random.Range(0, enemySelection.Count);
                GameObject selection = enemySelection[randomSelectionNumber];
                return selection;
        } 

        static List<GameObject> GetEnemyOptions(){
            Object[] data = Resources.LoadAll("Enemys", typeof(GameObject));

            List<GameObject> enemys = new List<GameObject>();
            foreach ( GameObject enemy in data) {
                if (PrefabUtility.GetPrefabAssetType(enemy) !=  PrefabAssetType.NotAPrefab) {
                    enemys.Add(enemy);
                } else {
                    Debug.Log(enemy.name + " is not a enemy");
                }
            }
            return enemys;
        }
    }



    static class ActivateDifficulty 
    {
        static Dictionary<string, System.Func<int>> ConsumeDifficultyOptionsMethods = new Dictionary<string, System.Func<int>>();
        static List<string> ConsumeDifficultyOptionsMethodsKeys;


        public static void ActivateDifficultyConstructor() {
            ConsumeDifficultyOptionsMethods.Add("SpawnEnemy",  SpawnEnemy.SpawnRandomEnemy);
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



