﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI;
using Unity.AI.Navigation;
using UnityEditor;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] public GameObject enemyPrefab;

    static public GameObject enemyStorageFolder;
    static List<GameObject> enemyCollection;

    public static NavMeshData navMeshData;
    public NavMeshAgent navMeshAgent;

    public Rigidbody rigidBody;


    public bool behaviorActive = false;
    public Coroutine behaviorCoroutine;

    public bool alive = true;
    public GameObject target;

    public int killScoreValue;
    public float health;
    public AudioSource deathSound;



    static Enemy() {
        Enemy.enemyCollection = new List<GameObject>();
    }

    public abstract IEnumerator ActivateBehavior();

    public void MoveTo(Vector3 position)
    {
        if (alive == true && navMeshAgent.enabled == true)
            {
                navMeshAgent.destination = position;

            }
    }


    public void takeDamage(float amount, GameObject Player)
    {
        health -= amount;
        if (health <= 0)
        {
            StopCoroutine(behaviorCoroutine);
            alive = false;
            Player.GetComponent<PlayerStatus>().AddScore(killScoreValue);
            deathSound.Play();
            Destroy(gameObject);
        }
    }



        
    public static  int SpawnRandomEnemy(Vector3 position){
        if (enemyStorageFolder == null) {
            Enemy.enemyStorageFolder = GameObject.Find("BotsFolder");
            if (enemyStorageFolder == null) {
                Debug.LogError("'BotsFolder' GameObject does not exist or name has been changed");
                return 0;
            }
        }

        GameObject enemy = selectRandomEnemyType();
        GameScript.coroutineHandler.callCoroutine(Spawn(enemy, position));
        return 100;
    }


    public static IEnumerator Spawn(GameObject enemy, Vector3 position) {
        while (Enemy.navMeshData == null)
        {
                yield return new WaitForSeconds(.1f);
        }
        while (Enemy.navMeshData.sourceBounds.size.x <= 0f && Enemy.navMeshData.sourceBounds.size.z <= 0)
        {
                yield return new WaitForSeconds(.1f);
        }

        float x = Random.Range(-25, 25);
        float z = Random.Range(-25, 25);

        Vector3 randomEnemyPosition = position + new Vector3(x,2,z);
        GameObject newEnemy = Object.Instantiate(enemy, randomEnemyPosition, Quaternion.identity, enemyStorageFolder.transform);
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
                enemys.Add(enemy);
        }
        Enemy.enemyCollection = enemys;
    }

}
