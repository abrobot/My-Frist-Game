using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI;
using Unity.AI.Navigation;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    static public GameObject enemyStorageFolder;
    static List<GameObject> enemyCollection;

    public static NavMeshData navMeshData;

    [SerializeField] public GameObject enemyPrefab;

    [SerializeField] public float Health = 50f;
    [SerializeField] AudioSource deathSound;

    static Enemy() {
        Enemy.enemyCollection = new List<GameObject>();
    }


    public void takeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            deathSound.Play();
            Destroy(gameObject);
        }
    }
        
    public static  int SpawnRandomEnemy(){
        if (enemyStorageFolder == null) {
            Enemy.enemyStorageFolder = GameObject.Find("BotsFolder");
            if (enemyStorageFolder == null) {
                Debug.LogError("'BotsFolder' GameObject does not exist or name has been changed");
                return 0;
            }
        }

        GameObject enemy = selectRandomEnemyType();
        GameScript.coroutineHandler.callCoroutine(Spawn(enemy));
        return 100;
    }


    public static IEnumerator Spawn(GameObject enemy) {
        print("spawn");
        while (Enemy.navMeshData == null)
        {
            print(111);
                yield return new WaitForSeconds(.1f);
        }
        print(enemyStorageFolder);
        GameObject newEnemy = Object.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Plane), new Vector3(30, 0, 0), Quaternion.identity, enemyStorageFolder.transform);
    }


    static GameObject selectRandomEnemyType(){
        if (enemyCollection.Count == 0) {
            RetrieveEnemyCollection();
        }
        
        int randomSelectionNumber = Random.Range(0, enemyCollection.Count);
        GameObject selection = enemyCollection[randomSelectionNumber];
        return selection;
    }

    static void RetrieveEnemyCollection(){
        Object[] data = Resources.LoadAll("Enemys", typeof(GameObject));

        List<GameObject> enemys = new List<GameObject>();
        foreach ( GameObject enemy in data) {
            if (PrefabUtility.GetPrefabAssetType(enemy) !=  PrefabAssetType.NotAPrefab) {
                enemys.Add(enemy);
            } else {
                Debug.Log(enemy.name + " is not a enemy");
            }
        }
        Enemy.enemyCollection = enemys;
    }

}