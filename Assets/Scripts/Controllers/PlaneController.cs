using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PathCreation;
using TMPro;

public class PlaneController : MonoBehaviour
{
    public enum PlaneState
    {
        Wandering,
        Parking,
        Parked,
        Taxiing,
        Flying
    }
    [SerializeField] private float wanderRadius = 20f; // Radius for point selection.
    [SerializeField] private float wanderInterval = 5f; // Time interval for picking a new point.
    [SerializeField] private PlaneDataSO planeData;

    public PlaneState currentState;
    private TextMeshPro identifier;
    private NavMeshAgent navMeshAgent;
    private GameObject lights;
    private HangarController ascociatedHangar;
    private TakeOffPointController ascociatedTakeOffPoint;
    private PathCreator ascociatedPath;
    private float distanceTravelled = 0f;
    private float speed = 5f;
    public int planeId;

    public bool isParked;
    public bool isAtTakeOffPoint;
    public bool isFollowingPath;
    private float timer;

    void Awake()
    {

        isParked = false;
        isAtTakeOffPoint = false;
        isFollowingPath = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        identifier = GetComponentInChildren<TextMeshPro>();
        lights = transform.Find("Lights").gameObject;
        timer = wanderInterval;
    }

    void Start()
    {
        GameManager.Instance.planesAreWandering.AddListener(() => currentState = PlaneState.Wandering);
        GameManager.Instance.parkAllPlanes.AddListener(() => currentState = PlaneState.Parking);
        GameManager.Instance.planesAreTakingOff.AddListener(() => currentState = PlaneState.Taxiing);
        GameManager.Instance.togglePlaneLights.AddListener(ToggleLights);
    }

    void Update()
    {
        switch (currentState)
        {
            case PlaneState.Wandering:
                handleWandering();
                break;
            case PlaneState.Parking:
                handleParking();
                break;
            case PlaneState.Parked:
                break;
            case PlaneState.Taxiing:
                handleTakeOff();
                break;
            case PlaneState.Flying:
                FollowPath();
                break;
        }
    }

    private void handleWandering()
    {
        if (isParked)
        {
            isParked = false;
        }

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
        if (isParked) return;

        // Assign the associated hangar based on the planeId
        ascociatedHangar = GameManager.Instance.hangars[planeId];

        // If a hanger is found with the same index as the planeId, park the plane in the associated hangar
        if (!ascociatedHangar) return;
        navMeshAgent.destination = ascociatedHangar.transform.position;

        // If a path is calculated, and the plane is within the stopping distance, it is parked
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            currentState = PlaneState.Parked;
            Debug.Log($"{gameObject.name} is parked at {ascociatedHangar.gameObject.name}");
        }
    }

    private void handleTakeOff()
    {

        if (!isAtTakeOffPoint)
        {
            // Assign the associated takeOffPoint & path based on the planeId
            ascociatedTakeOffPoint = GameManager.Instance.takeOffPoints[planeId];
            ascociatedPath = ascociatedTakeOffPoint.path;

            // If a takeOffPoint is found with the same index as the planeId, navigate to the associated takeOffPoint
            if (!ascociatedTakeOffPoint) return;
            navMeshAgent.destination = ascociatedTakeOffPoint.transform.position;
        }


        // If a path is calculated, and the plane is within the stopping distance, it is parked
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            isAtTakeOffPoint = true;
            navMeshAgent.enabled = false;
            currentState = PlaneState.Flying;
            Debug.Log($"{gameObject.name} is taking off from {ascociatedTakeOffPoint.gameObject.name}");
        }
    }

    void FollowPath()
    {
        if (ascociatedPath != null)
        {
            // Move along the path
            distanceTravelled += speed * Time.deltaTime;
            transform.position = ascociatedPath.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = ascociatedPath.path.GetRotationAtDistance(distanceTravelled);
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
