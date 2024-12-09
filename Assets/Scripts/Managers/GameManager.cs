using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlaneController[] planes;
    [HideInInspector] public HangarController[] hangars;
    [HideInInspector] public UnityEvent planesAreWandering = new UnityEvent();
    [HideInInspector] public UnityEvent parkAllPlanes = new UnityEvent();
    [HideInInspector] public UnityEvent allPlanesAreParked = new UnityEvent();
    [HideInInspector] public UnityEvent togglePlaneLights = new UnityEvent();

    private bool isParkingInProgress = false;

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

        WanderPlanes();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ParkAllPlanes();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            TogglePlaneLights();
        }

        if (!isParkingInProgress) return;
        CheckParkedPlanes();

    }

    public void WanderPlanes()
    {
        planesAreWandering.Invoke();
    }

    public void ParkAllPlanes()
    {
        parkAllPlanes.Invoke();
        isParkingInProgress = true;
    }

    public void TogglePlaneLights()
    {
        togglePlaneLights.Invoke();
    }

    private void CheckParkedPlanes()
    {
        foreach (PlaneController plane in planes)
        {
            if (!plane.isParked)
            {
                return;
            }
        }

        allPlanesAreParked.Invoke();
        isParkingInProgress = false;
        Debug.Log("All planes are parked!");
    }
}
