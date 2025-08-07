using UnityEngine;
using TMPro;

public class SettingsUIController : MonoBehaviour
{
    [Header("Light IP Fields")]
    public TMP_InputField light1Input;
    public TMP_InputField light2Input;

    [Header("Car & Alexa Fields")]
    public TMP_InputField carUrlInput;
    public TMP_InputField lockPATInput;

    void Start()
    {
        // Populate fields with current values
        var settings = AppSettingsManager.Instance;
        light1Input.text = settings.light1IP;
        light2Input.text = settings.light2IP;
        carUrlInput.text = settings.carBaseUrl;
        lockPATInput.text = settings.lockPAT;

        // Set up listeners for real-time updates
        light1Input.onEndEdit.AddListener(val => settings.light1IP = val);
        light2Input.onEndEdit.AddListener(val => settings.light2IP = val);
        carUrlInput.onEndEdit.AddListener(val => settings.carBaseUrl = val);
        lockPATInput.onEndEdit.AddListener(val => settings.lockPAT = val);
    }
}
