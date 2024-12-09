using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image planeStateIndicatorIcon;
    [SerializeField] private TextMeshProUGUI planeStateIndicatorText;
    [SerializeField] private UIPlaneStateDataSO planeStateData;

    void Start()
    {
        GameManager.Instance.planesAreWandering.AddListener(handlePlanesAreWandering);
        GameManager.Instance.parkAllPlanes.AddListener(handleParkAllPlanes);
        GameManager.Instance.allPlanesAreParked.AddListener(handleAllPlanesAreParked);
    }

    private void handlePlanesAreWandering()
    {
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[0].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[0].TextIndicator;
    }

    private void handleParkAllPlanes()
    {
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[1].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[1].TextIndicator;
    }

    private void handleAllPlanesAreParked()
    {
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[2].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[2].TextIndicator;
    }
}
