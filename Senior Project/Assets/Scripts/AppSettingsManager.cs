using UnityEngine;

public class AppSettingsManager : MonoBehaviour
{
    public static AppSettingsManager Instance { get; private set; }

    [Header("💡 Light Settings")]
    public string light1IP = "192.168.1.113";
    public string light2IP = "192.168.1.117";

    [Header("🚗 Car Settings")]
    public string carBaseUrl = "http://192.168.4.1";

    [Header("🔐 Alexa Settings")]
    public string lockPAT = "a31ab67b-ef43-4e50-816c-6016f2a8c9e6";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  // Persist between scenes if needed
    }
}
