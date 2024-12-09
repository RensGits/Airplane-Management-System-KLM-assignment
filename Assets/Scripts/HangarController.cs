using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class HangarController : MonoBehaviour
{
    private TextMeshPro identifier;
    public int hangarId;
    
    private void Awake()
    {
        identifier = GetComponentInChildren<TextMeshPro>();
    }

    public void UpdateIdentifier(int newId)
    {
        hangarId = newId;

        if (!identifier) return;

        identifier.text = $"{hangarId}"; // Update the TextMeshPro text
    }
}
