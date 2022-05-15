using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using Unity.AI;
using Unity.AI.Navigation;


public class Difficulty : OneInstance
{
    public static Difficulty instance {get; set;}

    public int DifficultyPoints;
    public GameObject player;

    public List<EnemyGroup> enemyGroups = new List<EnemyGroup>();

    public int GroupMaxDifficulty = 500;
    public int GroupMinDifficulty = 300;

    int MaxDifficultyPointsCanGenerate = 400;
    int MinDifficultyPointsCanGenerate = 300;

     int maxAmountOfGroups = 0;

    void Awake()
    {
        if (instance) {
            Debug.LogWarning("Did you mean to make second instance of type. Type extends OneInstance");
        } else {
            instance = this;
        }

        OneInstance.AddInstance(this.ToString(), this);

        DifficultyPoints = 300;

        StartCoroutine(DifficultyPointsGenerator());
        StartCoroutine(Activate(player));

    }

    override public void ResetInstance() {
        OneInstance.AllInstances[this.ToString()] =  new Difficulty();
    }
    

    public IEnumerator Activate(GameObject player)
    {
        float SixtySecondsLoopTime = 0f;

        while (true)
        {
            if (enemyGroups.Count < maxAmountOfGroups)
            {
                for (int i = 0; DifficultyPoints >= GroupMinDifficulty; i++)
                {

                    EnemyGroup enemyGroup = new EnemyGroup();
                    enemyGroup.GenerateGroup(player);

                    enemyGroups.Add(enemyGroup);
                    yield return new WaitForSeconds(.1f);
                }
            }

            yield return new WaitForSeconds(1);
            SixtySecondsLoopTime += 1;
            if (SixtySecondsLoopTime == 60f)
            {
                SixtySecondsLoopTime = 0f;
                maxAmountOfGroups += 1;
                MaxDifficultyPointsCanGenerate += 200;
                MinDifficultyPointsCanGenerate += 100;
            }
        }
    }


    public void RemoveEnemyGroup(EnemyGroup enemyGroup) {
        enemyGroups.Remove(enemyGroup);
    }

    public IEnumerator DifficultyPointsGenerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(25, 40));
            DifficultyPoints += Random.Range(MinDifficultyPointsCanGenerate, MaxDifficultyPointsCanGenerate);
            //DifficultyPoints += Random.Range(0, 0);
        }
    }
}



