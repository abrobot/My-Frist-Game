using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField] private GameObject target;


    // Update is called once per frame
    void Update()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in transform)
        {
            Enemy enemy = child.gameObject.GetComponent<Enemy>();
            if (enemy.behaviorActive != true)
            {
                enemy.target = target;
                enemy.behaviorCoroutine = enemy.StartCoroutine(enemy.ActivateBehavior());
            }
        }
    }
}