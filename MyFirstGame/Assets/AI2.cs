using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI2 : MonoBehaviour
{
    [SerializeField] private Transform Target;


    // Update is called once per frame
    void Update()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in transform)
        {
            Enemy enemy = child.gameObject.GetComponent<Enemy>();
            if (enemy.Health != 0)
            {
                NavMeshAgent navMeshAgent = child.gameObject.GetComponent<NavMeshAgent>();
                navMeshAgent.destination = Target.position;
            }
        }

    }
}
