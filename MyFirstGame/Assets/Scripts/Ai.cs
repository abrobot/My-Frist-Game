using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai : MonoBehaviour
{
    [SerializeField] private Transform Target;
    private  NavMeshAgent navMeshAgent;
    private void Awake() {
        //navMeshAgent = GetComponent<NavMeshAgent>();
    } 


    void Update()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in transform)
        {
            navMeshAgent = child.gameObject.GetComponent<NavMeshAgent>();
            Debug.Log(child.gameObject.GetComponent<NavMeshAgent>());
            navMeshAgent.destination = Target.position;
        }
    }
}