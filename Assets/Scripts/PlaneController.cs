using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PlaneController : MonoBehaviour
{
    [SerializeField] private float wanderRadius = 20f; // Radius for point selection.
    [SerializeField] private float wanderInterval = 5f; // Time interval for picking a new point.

    [SerializeField] private GameObject hangar;

    [SerializeField] private PlaneDataSO planeData;

    private TextMeshPro identifier;
    private NavMeshAgent navMeshAgent;
    public int planeId;
    private float timer;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        identifier = GetComponentInChildren<TextMeshPro>();
        timer = wanderInterval;
        LogPlaneData();
    }

    private void Update()
    {
        // navMeshAgent.destination = hangar.transform.position;
        timer += Time.deltaTime;

        if (timer >= wanderInterval)
        {
            Vector3 newDestination = GetRandomPointOnNavMesh(transform.position, wanderRadius);
            navMeshAgent.SetDestination(newDestination);
            timer = 0;
        }
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 origin, float radius)
    {
        Vector3 forward = transform.forward; // Get the plane's forward direction
        float angle = Random.Range(-55f, 55f); // Limit the angle to 55 degrees left and right
        Quaternion rotation = Quaternion.Euler(0, angle, 0); // Rotate around the Y-axis
        Vector3 randomDirection = rotation * forward * Random.Range(0.5f * radius, radius); // Scale to radius

        // Project the random point onto the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            return hit.position; 
        }

        // If no valid point is found, return the agent's current position
        return transform.position;
    }

    private void LogPlaneData()
    {
        Debug.Log($"Plane type: {planeData.type}, Plane brand: {planeData.brand}");
    }

    public void UpdateIdentifier(int newId)
    {
        planeId = newId;

        if (!identifier) return;

        identifier.text = $"{planeId}"; // Update the TextMeshPro text
    }
}
