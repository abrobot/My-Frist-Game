using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai : MonoBehaviour
{
    public GameObject targetPlayer;
    public Target target;


    // Update is called once per frame
    void Update()
    {

    }

    void Start()
    {
        StartCoroutine(aiUpdateLoop());
    }



    IEnumerator aiUpdateLoop()
    {
        while (true)
        {
            foreach (EnemyGroup enemyGroup in Difficulty.instance.enemyGroups)
            {
                bool isDay = (Game.GameInstance.dayNightCycle.dayNight == DayNightCycle.DayNight.Day);
                Vector3 groupCenter = CalcCenter(enemyGroup.enemies.ToArray());
                enemyGroup.groupBounds.center = groupCenter;
                DrawBounds(enemyGroup.groupBounds, .1f);

                foreach (GameObject enemyGameObject in enemyGroup.enemies)
                {
                    if (!enemyGameObject)
                    {
                        continue;
                    }

                    Enemy enemy = enemyGameObject.GetComponent<Enemy>();
                    if (enemyGroup.groupBounds.Contains(targetPlayer.transform.position) | isDay == false)
                    {
                        if (enemy.behaviorActive != true)
                        {

                            if (enemy.target == null)
                            {
                                enemy.target = new Target(targetPlayer.transform.position, targetPlayer);
                            }
                            enemy.target.objectTarget = targetPlayer;
                            enemy.target.enabled = true;

                            enemy.behaviorCoroutine = enemy.StartCoroutine(enemy.ActivateBehavior());
                        }
                    }
                    else if (enemy.target != null)
                    {
                        float DistFromPlayer = Mathf.Sqrt(enemyGroup.groupBounds.SqrDistance(targetPlayer.transform.position));

                        if (DistFromPlayer > enemyGroup.groupSightDist && isDay == true)
                        {
                            float enemysDistFromGroup = Mathf.Sqrt((enemy.transform.position - enemyGroup.groupBounds.center).sqrMagnitude);
                            if (enemysDistFromGroup > 30)
                            {
                                enemy.target.objectTarget = null;
                                enemy.target.position = enemyGroup.groupBounds.center;
                            }
                            else
                            {
                                enemy.target.enabled = false;
                                enemy.StopMove();
                            }
                        }
                        else
                        {
                            if (enemy.behaviorActive == true && isDay == true)
                            {
                                enemy.target.objectTarget = targetPlayer;
                                enemy.target.enabled = true;
                            }
                        }
                    }

                }

            }

            yield return new WaitForSeconds(.5f);
        }
    }


    






    Vector3 CalcCenter(GameObject[] objs)
    {
        var min = Vector3.one * Mathf.Infinity;
        var max = Vector3.one * Mathf.NegativeInfinity;
        foreach (GameObject obj in objs)
        {
            if (!obj)
            {
                continue;
            }
            Bounds box = obj.GetComponent<Renderer>().bounds; // or use obj.collider.bounds
            min = Vector3.Min(min, box.min); // expand min to encapsulate bounds.min
            max = Vector3.Max(max, box.max); // expand max to encapsulate bounds.max
        }
        return (max + min) / 2;
    }


    static public void DrawBounds(Bounds b, float delay = 5)
    {
        // bottom
        var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
        var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
        var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
        var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

        Debug.DrawLine(p1, p2, Color.blue, delay);
        Debug.DrawLine(p2, p3, Color.red, delay);
        Debug.DrawLine(p3, p4, Color.yellow, delay);
        Debug.DrawLine(p4, p1, Color.magenta, delay);

        // top
        var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
        var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
        var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
        var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

        Debug.DrawLine(p5, p6, Color.blue, delay);
        Debug.DrawLine(p6, p7, Color.red, delay);
        Debug.DrawLine(p7, p8, Color.yellow, delay);
        Debug.DrawLine(p8, p5, Color.magenta, delay);

        // sides
        Debug.DrawLine(p1, p5, Color.white, delay);
        Debug.DrawLine(p2, p6, Color.gray, delay);
        Debug.DrawLine(p3, p7, Color.green, delay);
        Debug.DrawLine(p4, p8, Color.cyan, delay);
    }



}