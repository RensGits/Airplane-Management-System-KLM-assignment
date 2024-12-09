using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PlaneController : MonoBehaviour
{
    private enum PlaneState
    {
        Wandering,
        Parking
    }
    [SerializeField] private float wanderRadius = 20f; // Radius for point selection.
    [SerializeField] private float wanderInterval = 5f; // Time interval for picking a new point.
    [SerializeField] private PlaneDataSO planeData;

    private PlaneState currentState;
    private TextMeshPro identifier;
    private NavMeshAgent navMeshAgent;
    private GameObject lights;
    private HangarController ascociatedHangar;
    public int planeId;
    private float timer;


    private void Awake()
    {   
        currentState = PlaneState.Wandering;
        navMeshAgent = GetComponent<NavMeshAgent>();
        identifier = GetComponentInChildren<TextMeshPro>();
        lights = transform.Find("Lights").gameObject;
        
        GameManager.Instance.parkAllPlanes.AddListener(() => currentState = PlaneState.Parking);
        GameManager.Instance.togglePlaneLights.AddListener(ToggleLights);
        
        timer = wanderInterval;
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlaneState.Wandering:
                handleWandering();
                break;
            case PlaneState.Parking:
                handleParking();
                break;
        }
    }

    private void handleWandering()
    {
        timer += Time.deltaTime;

        if (timer >= wanderInterval)
        {
            Vector3 newPos = GetRandomPointOnNavMesh(transform.position, wanderRadius);
            navMeshAgent.SetDestination(newPos);
            timer = 0;
        }
    }

    private void handleParking()
    {
        // Assign the associated hangar based on the planeId
        ascociatedHangar = GameManager.Instance.hangars[planeId];

        // If a hanger is found with the same index as the planeId, park the plane in the associated hangar
        if (!ascociatedHangar) return;
        navMeshAgent.destination = ascociatedHangar.transform.position;
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

    public void UpdateIdentifier(int newId)
    {
        planeId = newId;

        if (!identifier) return;

        // Update the identifier text to match the planeId and add 1 to make it more palletable
        identifier.text = $"{planeId + 1}"; 
    }

    public void ToggleLights()
    {
        if (!lights) return;
        lights.SetActive(!lights.activeSelf);
    }
}
