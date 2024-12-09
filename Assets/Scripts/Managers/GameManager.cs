using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlaneController[] planes;
    [HideInInspector] public HangarController[] hangars;
    [HideInInspector] public TakeOffPointController[] takeOffPoints;
    [HideInInspector] public PathCreator[] paths;
    [HideInInspector] public UnityEvent planesAreWandering = new UnityEvent();
    [HideInInspector] public UnityEvent parkAllPlanes = new UnityEvent();
    [HideInInspector] public UnityEvent allPlanesAreParked = new UnityEvent();
    [HideInInspector] public UnityEvent planesAreTakingOff = new UnityEvent();
    [HideInInspector] public UnityEvent planesAreFlying = new UnityEvent();
    [HideInInspector] public UnityEvent togglePlaneLights = new UnityEvent();

    private bool isParkingInProgress = false;
    private bool isTaxiInProgress = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find all PlaneController & HangarController components in the scene
        planes = FindObjectsOfType<PlaneController>();
        hangars = FindObjectsOfType<HangarController>();
        takeOffPoints = FindObjectsOfType<TakeOffPointController>();

        // Assign a unique planeId to each PlaneController based on its index
        for (int i = 0; i < planes.Length; i++)
        {
            planes[i].UpdateIdentifier(i);
        }

        // Assign a unique hangarId to each HangarController based on its index
        for (int i = 0; i < hangars.Length; i++)
        {
            hangars[i].UpdateIdentifier(i);
        }

        // Assign a unique takeOffPointId to each TakeOffPointController based on its index
        for (int i = 0; i < takeOffPoints.Length; i++)
        {
            takeOffPoints[i].UpdateIdentifier(i);
        }

        WanderPlanes();
    }

    void Update()
    {
        if (isParkingInProgress)
        {
            CheckParkedPlanes();
        }

        if(isTaxiInProgress)
        {
            CheckFlyingPlanes();
        }
        
    }

    public void WanderPlanes()
    {
        Debug.Log("Planes are wandering!");	
        planesAreWandering.Invoke();
    }

    public void ParkAllPlanes()
    {
        parkAllPlanes.Invoke();
        isParkingInProgress = true;
    }

    public void PlanesAreTakingOff()
    {
        planesAreTakingOff.Invoke();
        isTaxiInProgress = true;
    }

    public void PlanesAreFlying()
    {
        planesAreFlying.Invoke();
    }

    public void TogglePlaneLights()
    {
        togglePlaneLights.Invoke();
    }

    private void CheckParkedPlanes()
    {
        foreach (PlaneController plane in planes)
        {
            if (!plane.currentState.Equals(PlaneController.PlaneState.Parked))
            {
                return;
            }
        }

        allPlanesAreParked.Invoke();
        isParkingInProgress = false;
        Debug.Log("All planes are parked!");
    }

    private void CheckFlyingPlanes()
    {
        foreach (PlaneController plane in planes)
        {
            if (!plane.currentState.Equals(PlaneController.PlaneState.Flying))
            {
                return;
            }
        }

        planesAreFlying.Invoke();
        isTaxiInProgress = false;
        Debug.Log("All planes are flying!");
    }
}
