using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUIPlaneStateData", menuName = "UI/PlaneStateData")]

[Serializable]
public class UIPlaneStateDataSO : ScriptableObject
{
    [Serializable]
    public class PlaneStateAttributes
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string TextIndicator { get; private set; }
    }

    [field: SerializeField] public List<PlaneStateAttributes> planeUIElements { get; private set; }
}
