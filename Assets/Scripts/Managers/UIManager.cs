using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Image planeStateIndicatorIcon;
    [SerializeField] private TextMeshProUGUI planeStateIndicatorText;
    [SerializeField] private UIPlaneStateDataSO planeStateData;
    [SerializeField] private Button parkWanderButton;
    [SerializeField] private TextMeshProUGUI parkWanderButtonText;
    [SerializeField] private Button lightsButton;
    [SerializeField] private TextMeshProUGUI lightsButtonText;
    [SerializeField] private Button takeOffButton;

    [SerializeField] private GameObject planeInfoPanel;
    [SerializeField] private TextMeshProUGUI planeId;
    [SerializeField] private TextMeshProUGUI planeType;
    [SerializeField] private TextMeshProUGUI planeBrand;
    [SerializeField] private TextMeshProUGUI planeState;

    // Create singleton and add listeners to the game manager
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

        GameManager.Instance.planesAreWandering.AddListener(HandlePlanesAreWandering);
        GameManager.Instance.parkAllPlanes.AddListener(HandleParkAllPlanes);
        GameManager.Instance.planesAreTakingOff.AddListener(HandlePlanesAreTakingOff);
        GameManager.Instance.allPlanesAreParked.AddListener(HandleAllPlanesAreParked);
        GameManager.Instance.planesAreFlying.AddListener(HandleAllPlanesAreFlying);
        GameManager.Instance.enableLights.AddListener(HandleLightsAreOn);
        GameManager.Instance.disableLights.AddListener(HandleLightsAreOff);
    }

    private void HandlePlanesAreWandering()
    {
        EnableWanderParkToggle();

        // Adds the appropriate listener to the button
        parkWanderButton.onClick.RemoveAllListeners();
        parkWanderButton.onClick.AddListener(() => GameManager.Instance.ParkAllPlanes());

        // Updates the UI elements
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[0].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[0].TextIndicator;
        parkWanderButtonText.text = "Park";
    }

    private void HandleParkAllPlanes()
    {
        EnableWanderParkToggle();

        // Adds the appropriate listener to the button
        parkWanderButton.onClick.RemoveAllListeners();
        parkWanderButton.onClick.AddListener(() => GameManager.Instance.WanderPlanes());

        // Updates the UI elements
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[1].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[1].TextIndicator;
        parkWanderButtonText.text = "Wander";
    }

    private void HandleLightsAreOff()
    {
        // Adds the appropriate listener to the button
        lightsButton.onClick.RemoveAllListeners();
        lightsButton.onClick.AddListener(() => GameManager.Instance.EnableLights());

        // Updates the UI elements
        lightsButtonText.text = "Lights On";
    }

    private void HandleLightsAreOn()
    {
        // Adds the appropriate listener to the button
        lightsButton.onClick.RemoveAllListeners();
        lightsButton.onClick.AddListener(() => GameManager.Instance.DisableLights());

        // Updates the UI elements
        lightsButtonText.text = "Lights Off";
    }

    // Updated the top Planes State Indicator to show that all planes are parked
    private void HandleAllPlanesAreParked()
    {
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[2].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[2].TextIndicator;
    }

    // Updated the top Planes State Indicator to show that all planes are taking off
    private void HandlePlanesAreTakingOff()
    {
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[3].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[3].TextIndicator;
        DisableWanderParkToggle();
    }

    // Updated the top Planes State Indicator to show that all planes are flying
    private void HandleAllPlanesAreFlying()
    {
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[4].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[4].TextIndicator;
    }
    
    private void DisableWanderParkToggle()
    {
        parkWanderButton.interactable = false;
        takeOffButton.interactable = false;
    }

    private void EnableWanderParkToggle()
    {
        if (parkWanderButton.interactable && takeOffButton.interactable) return;
        parkWanderButton.interactable = true;
        takeOffButton.interactable = true;
    }

    public void EnablePlaneInfoPanel(int id, string type, string brand, string state)
    {
        planeInfoPanel.SetActive(true);
        planeId.text = $"Plane ID: {id + 1}";
        planeType.text = $"Plane Type: {type}";
        planeBrand.text = $"Plane Brand: {brand}";
        planeState.text = $"Currently: {state}";
    }

    public void DisablePlaneInfoPanel()
    {
        planeInfoPanel.SetActive(false);
    }
}
