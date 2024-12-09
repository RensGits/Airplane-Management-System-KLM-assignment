using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image planeStateIndicatorIcon;
    [SerializeField] private TextMeshProUGUI planeStateIndicatorText;
    [SerializeField] private UIPlaneStateDataSO planeStateData;
    [SerializeField] private Button parkWanderButton;
    [SerializeField] private TextMeshProUGUI parkWanderButtonText;

    void Awake()
    {
        GameManager.Instance.planesAreWandering.AddListener(handlePlanesAreWandering);
        GameManager.Instance.parkAllPlanes.AddListener(handleParkAllPlanes);
        GameManager.Instance.planesAreTakingOff.AddListener(handlePlanesAreTakingOff);
        GameManager.Instance.allPlanesAreParked.AddListener(handleAllPlanesAreParked);
        GameManager.Instance.planesAreFlying.AddListener(handleAllPlanesAreFlying);
    }
    
    private void handlePlanesAreWandering()
    {   
        Debug.Log("Planes are wandering from UIManager");	
        // Adds the appropriate listener to the button
        parkWanderButton.onClick.RemoveAllListeners();
        parkWanderButton.onClick.AddListener(() => GameManager.Instance.ParkAllPlanes());

        // Updates the UI elements
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[0].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[0].TextIndicator;
        parkWanderButtonText.text = "Park";
    }

    private void handleParkAllPlanes()
    {
        // Adds the appropriate listener to the button
        parkWanderButton.onClick.RemoveAllListeners();
        parkWanderButton.onClick.AddListener(() => GameManager.Instance.WanderPlanes());
        
        // Updates the UI elements
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[1].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[1].TextIndicator;
        parkWanderButtonText.text = "Wander";
    }


    private void handleAllPlanesAreParked()
    {
        Debug.Log("All planes are parked from UIManager");
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[2].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[2].TextIndicator;
    }

    private void handlePlanesAreTakingOff()
    {
        Debug.Log("Planes are taking off from UIManager");
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[3].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[3].TextIndicator;
    }

    private void handleAllPlanesAreFlying()
    {
        Debug.Log("All planes are flying from UIManager");	
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[4].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[4].TextIndicator;
    }
}
