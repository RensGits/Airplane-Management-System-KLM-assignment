using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PathCreation;
using TMPro;

public class PlaneController : MonoBehaviour
{
    [SerializeField] private float wanderRadius = 20f; // Radius for point selection.
    [SerializeField] private float wanderInterval = 5f; // Time interval for picking a new point.
    [SerializeField] private PlaneDataSO planeData;

    private TextMeshPro identifier;
    private NavMeshAgent navMeshAgent;
    private GameObject lights;
    private GameObject trails;
    private HangarController ascociatedHangar;
    private TakeOffPointController ascociatedTakeOffPoint;
    private PathCreator ascociatedPath;
    private float distanceTravelled = 0f;
    private float speed = 5f;
    private float minSpeed = 0.5f;
    private float landingRunwayLength = 10f;
    public int planeId;
    public bool isAtTakeOffPoint;
    private float timer;

    public enum PlaneState
    {
        Wandering,
        Parking,
        Parked,
        Taxiing,
        Flying,
        FinishedFlying
    }
    public PlaneState currentState;

    void Awake()
    {
        isAtTakeOffPoint = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        identifier = GetComponentInChildren<TextMeshPro>();
        lights = transform.Find("Lights").gameObject;
        trails = transform.Find("Trails").gameObject;
        timer = wanderInterval;
    }

    void Start()
    {
        GameManager.Instance.planesAreWandering.AddListener(() => currentState = PlaneState.Wandering);
        GameManager.Instance.parkAllPlanes.AddListener(() => currentState = PlaneState.Parking);
        GameManager.Instance.planesAreTakingOff.AddListener(() => currentState = PlaneState.Taxiing);
        GameManager.Instance.enableLights.AddListener(HandleEnableLights);
        GameManager.Instance.disableLights.AddListener(HandleDisableLights);
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
            case PlaneState.FinishedFlying:
                return;
        }
    }

    private void handleWandering()
    {
        timer += Time.deltaTime;

        if (timer >= wanderInterval && navMeshAgent.enabled)
        {
            Vector3 newPos = GetRandomPointOnNavMesh(transform.position, wanderRadius);
            navMeshAgent.SetDestination(newPos);
            timer = 0;
        }
    }

    private void handleParking()
    {
        if (currentState == PlaneState.Parked) return;

        // Assign the associated hangar based on the planeId
        ascociatedHangar = GameManager.Instance.hangars[planeId];

        // If a hanger is found with the same index as the planeId, park the plane in the associated hangar
        if (!ascociatedHangar) return;
        navMeshAgent.destination = ascociatedHangar.transform.position;

        // If a path is calculated, and the plane is within the stopping distance, it is parked
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            currentState = PlaneState.Parked;
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
        }
    }

    private void FollowPath()
    {
        if (ascociatedPath != null && distanceTravelled <= (ascociatedPath.path.length + landingRunwayLength))
        {

            // Calculate the remaining distance to the end of the path
            float remainingDistance = ascociatedPath.path.length + landingRunwayLength - distanceTravelled;

            // Slow down when within a certain range of the end of the path
            if (remainingDistance <= landingRunwayLength)
            {
                // Calculate a speed multiplier based on remaining distance
                float slowDownFactor = Mathf.Clamp01(remainingDistance / landingRunwayLength);
                float currentSpeed = Mathf.Max(speed * slowDownFactor, minSpeed); // Ensure it doesn't go below minSpeed
                distanceTravelled += currentSpeed * Time.deltaTime;
            }
            else
            {
                // Maintain normal speed
                distanceTravelled += speed * Time.deltaTime;
            }

            // Move the plane along the path
            transform.position = ascociatedPath.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = ascociatedPath.path.GetRotationAtDistance(distanceTravelled);
            // Enable the trails
            if (!trails.activeSelf)
            {
                trails.SetActive(true);
            }
        }
        else
        {
            OnFlyingFinished();
        }
    }

    private void OnFlyingFinished()
    {
        // Disable the trails
        if (trails.activeSelf)
        {
            trails.SetActive(false);
        }

        // Reset the distance travelled
        distanceTravelled = 0f;
        isAtTakeOffPoint = false;
        navMeshAgent.enabled = true;

        currentState = PlaneState.FinishedFlying;
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

    private void HandleEnableLights()
    {
        if (!lights) return;
        lights.SetActive(true);
    }

    private void HandleDisableLights()
    {
        if (!lights) return;
        lights.SetActive(false);
    }
}
