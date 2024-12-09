using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlaneController[] planes;
    private HangarController[] hangars;
    void Start()
    {
        // Find all PlaneController components in the scene
        planes = FindObjectsOfType<PlaneController>();
        hangars = FindObjectsOfType<HangarController>();

        // Assign a unique planeId to each PlaneController based on its index
        for (int i = 0; i < planes.Length; i++)
        {
            planes[i].UpdateIdentifier(i);
        }

        for (int i = 0; i < hangars.Length; i++)
        {
            hangars[i].UpdateIdentifier(i);
        }
    }

    void Update()
    {

    }
}
