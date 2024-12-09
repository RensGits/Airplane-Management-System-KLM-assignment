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

        // Update the identifier text to match the hangarId and add 1 to make it more palletable
        identifier.text = $"{hangarId + 1}"; 
    }
}
