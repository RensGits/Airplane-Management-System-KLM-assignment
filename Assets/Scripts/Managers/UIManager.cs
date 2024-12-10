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
    [SerializeField] private Button lightsButton;
    [SerializeField] private TextMeshProUGUI lightsButtonText;
    [SerializeField] private Button takeOffButton;


    void Awake()
    {
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
        lightsButton.onClick.RemoveAllListeners();
        lightsButton.onClick.AddListener(() => GameManager.Instance.EnableLights());

        lightsButtonText.text = "Lights On";
    }

    private void HandleLightsAreOn()
    {
        lightsButton.onClick.RemoveAllListeners();
        lightsButton.onClick.AddListener(() => GameManager.Instance.DisableLights());

        lightsButtonText.text = "Lights Off";
    }


    private void HandleAllPlanesAreParked()
    {
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[2].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[2].TextIndicator;
    }

    private void HandlePlanesAreTakingOff()
    {
        planeStateIndicatorIcon.sprite = planeStateData.planeUIElements[3].Icon;
        planeStateIndicatorText.text = planeStateData.planeUIElements[3].TextIndicator;
        DisableWanderParkToggle();
    }

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
}
