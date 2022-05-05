using System.Collections;
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

    public EnemyGroup enemyGroup;

    public static NavMeshData navMeshData;
    public NavMeshAgent navMeshAgent;

    public Rigidbody rigidBody;


    public bool behaviorActive = false;
    public Coroutine behaviorCoroutine;

    public bool alive = true;
    public Target target;

    public int killScoreValue;
    public float health;
    public AudioSource deathSound;



    static Enemy()
    {
        Enemy.enemyCollection = new List<GameObject>();
    }

    public abstract IEnumerator ActivateBehavior();

    public void MoveTo(Vector3 position)
    {
        if (alive == true && navMeshAgent.enabled == true && navMeshData != null)
        {
            navMeshAgent.destination = position;
        }
    }

    public void StopMove()
    {
        if (alive == true && navMeshAgent.enabled == true)
        {
            navMeshAgent.isStopped = true;
        }
    }

    public void takeDamage(float amount, GameObject Player)
    {
        health -= amount;
        if (health <= 0)
        {
            if (behaviorActive)
            {
                StopCoroutine(behaviorCoroutine);
            }
            alive = false;
            Player.GetComponent<PlayerStatus>().AddScore(killScoreValue);
            deathSound.Play();

            enemyGroup.enemies.Remove(gameObject);
            if (enemyGroup.enemies.Count == 0)
            {
                Difficulty.instance.RemoveEnemyGroup(enemyGroup);
                enemyGroup = null;
            }
            Destroy(gameObject);
        }
    }




    public static int SpawnRandomEnemy(EnemyGroup enemyGroup, Vector3 position)
    {
        if (enemyStorageFolder == null)
        {
            Enemy.enemyStorageFolder = GameObject.Find("BotsFolder");
            if (enemyStorageFolder == null)
            {
                Debug.LogError("'BotsFolder' GameObject does not exist or name has been changed");
                return 0;
            }
        }

        GameObject enemy = selectRandomEnemyType();
        Game.coroutineHandler.callCoroutine(Spawn(enemyGroup, enemy, position));
        return 100;
    }


    public static IEnumerator Spawn(EnemyGroup enemyGroup, GameObject enemy, Vector3 position)
    {


        while (Enemy.navMeshData == null)
        {
            yield return new WaitForSeconds(.1f);
        }
        while (Enemy.navMeshData.sourceBounds.size.x <= 0f && Enemy.navMeshData.sourceBounds.size.z <= 0)
        {
            yield return new WaitForSeconds(.1f);
        }

        float x = Random.Range(-20, 20);
        float z = Random.Range(-20, 20);

        Vector3 randomEnemyPosition = position + new Vector3(x, 2, z);
        GameObject newEnemy = Object.Instantiate(enemy, randomEnemyPosition, Quaternion.identity, enemyStorageFolder.transform);
        enemyGroup.enemies.Add(newEnemy);
        newEnemy.GetComponent<Enemy>().enemyGroup = enemyGroup;
    }


    static GameObject selectRandomEnemyType()
    {
        if (enemyCollection.Count == 0)
        {
            RetrieveEnemyCollection();
        }

        int randomSelectionNumber = Random.Range(0, enemyCollection.Count);
        GameObject selection = enemyCollection[randomSelectionNumber];
        return selection;
    }

    static void RetrieveEnemyCollection()
    {
        Object[] data = Resources.LoadAll("Enemys", typeof(GameObject));

        List<GameObject> enemys = new List<GameObject>();
        foreach (GameObject enemy in data)
        {
            enemys.Add(enemy);
        }
        Enemy.enemyCollection = enemys;
    }
}


public class Target
{

    public GameObject objectTarget;
    public Bounds BoundsTarget;
    public Vector3 _position;

    public bool enabled = false;

    public Target(Vector3 position, GameObject gameObject = null)
    {
        this.position = position;
        this.objectTarget = gameObject;
    }

    public Vector3 position
    {
        get
        {
            if (objectTarget)
            {
                return objectTarget.transform.position;
            }
            else { return _position; }
        }
        set { _position = value; }
    }
}


