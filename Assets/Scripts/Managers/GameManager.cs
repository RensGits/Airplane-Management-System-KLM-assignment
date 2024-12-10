using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance;
    private PlaneController[] planes;
    [HideInInspector] public HangarController[] hangars;
    [HideInInspector] public TakeOffPointController[] takeOffPoints;    
    [HideInInspector] public UnityEvent planesAreWandering = new UnityEvent();
    [HideInInspector] public UnityEvent parkAllPlanes = new UnityEvent();
    [HideInInspector] public UnityEvent allPlanesAreParked = new UnityEvent();
    [HideInInspector] public UnityEvent planesAreTakingOff = new UnityEvent();
    [HideInInspector] public UnityEvent planesAreFlying = new UnityEvent();
    [HideInInspector] public UnityEvent enableLights = new UnityEvent();
    [HideInInspector] public UnityEvent disableLights = new UnityEvent();
    public bool areLightsOn = false;

    private enum AllPlanesTransitionState
    {
        IsParkingInProgress,
        IsTaxiInProgress,
        IsFlyingInProgress,
        None
    }

    private AllPlanesTransitionState planesState;   

    // Create singleton
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
        DisableLights();
    }

    void Update()
    {
        if (planesState == AllPlanesTransitionState.IsParkingInProgress)
        {
            CheckParkedPlanes();
        }

        if (planesState == AllPlanesTransitionState.IsTaxiInProgress)
        {
            CheckFlyingPlanes();
        }

        if (planesState == AllPlanesTransitionState.IsFlyingInProgress)
        {
            CheckFlyingFinished();
        }
    }

    public void WanderPlanes()
    {
        planesAreWandering.Invoke();
        planesState = AllPlanesTransitionState.None;
    }

    public void ParkAllPlanes()
    {
        parkAllPlanes.Invoke();
        planesState = AllPlanesTransitionState.IsParkingInProgress;
    }

    public void PlanesAreTakingOff()
    {
        planesAreTakingOff.Invoke();
        planesState = AllPlanesTransitionState.IsTaxiInProgress;
    }

    public void PlanesAreFlying()
    {
        planesAreFlying.Invoke();
        planesState = AllPlanesTransitionState.IsFlyingInProgress;
    }

    private void PlanesAreParked()
    {
        allPlanesAreParked.Invoke();
        planesState = AllPlanesTransitionState.None;
    }

    // Invoked the PlanesAreParked when all planes are parked
    private void CheckParkedPlanes()
    {
        foreach (PlaneController plane in planes)
        {
            if (plane.currentState != PlaneController.PlaneState.Parked)
            {
                return;
            }
        }

        PlanesAreParked();
    }

    // Invoked the PlanesAreFlying when all planes are flying
    private void CheckFlyingPlanes()
    {
        foreach (PlaneController plane in planes)
        {
            if (plane.currentState != PlaneController.PlaneState.Flying)
            {
                return;
            }
        }

        PlanesAreFlying();
    }

    // Invoked the WanderPlanes when all planes are finished flying / landed
    private void CheckFlyingFinished()
    {
        foreach (PlaneController plane in planes)
        {
            if (!plane.currentState.Equals(PlaneController.PlaneState.FinishedFlying))
            {
                return;
            }
        }

        WanderPlanes();
    }

    public void EnableLights()
    {
        areLightsOn = true;
        enableLights.Invoke();
    }

    public void DisableLights()
    {
        areLightsOn = false;
        disableLights.Invoke();
    }
}
