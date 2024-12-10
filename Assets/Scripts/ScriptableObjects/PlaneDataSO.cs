using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlane", menuName = "Planes/PlaneData")]

[Serializable]
public class PlaneDataSO: ScriptableObject
{
    [field: SerializeField] public string type { get; private set; }
    [field: SerializeField] public string brand { get; private set; }
}