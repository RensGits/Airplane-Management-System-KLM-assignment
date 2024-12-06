using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToHangar : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject hangar;

    private void Awake ()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.destination = hangar.transform.position;
    }
}
