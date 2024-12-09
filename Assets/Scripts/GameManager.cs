using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlaneController[] planes;
    public HangarController[] hangars;

    [HideInInspector] public UnityEvent parkAllPlanes = new UnityEvent();
    [HideInInspector] public UnityEvent togglePlaneLights = new UnityEvent();

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ParkAllPlanes();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            TogglePlaneLights();
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
    }

    public void ParkAllPlanes()
    {
        parkAllPlanes.Invoke();
    }

    public void TogglePlaneLights()
    {
        togglePlaneLights.Invoke();
    }
}
