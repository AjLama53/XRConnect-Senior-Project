using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Collections;

public class ESP32CarController : MonoBehaviour
{
    [Header("UI Buttons")]
    public GameObject forwardButton;
    public GameObject backwardButton;
    public GameObject leftButton;
    public GameObject rightButton;

    private string baseUrl => AppSettingsManager.Instance.carBaseUrl;

    private enum CarCmd { Forward = 1, Backward = 2, Left = 3, Right = 4, Stop = 5 }

    void Start()
    {
        SetupButton(forwardButton, CarCmd.Forward);
        SetupButton(backwardButton, CarCmd.Backward);
        SetupButton(leftButton, CarCmd.Left);
        SetupButton(rightButton, CarCmd.Right);
    }

    void SetupButton(GameObject button, CarCmd direction)
    {
        var trigger = button.GetComponent<EventTrigger>() ?? button.AddComponent<EventTrigger>();

        // OnPointerDown → Send movement command
        EventTrigger.Entry downEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        downEntry.callback.AddListener((_) => StartCoroutine(SendCommand((int)direction)));
        trigger.triggers.Add(downEntry);

        // OnPointerUp → Stop command
        EventTrigger.Entry upEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        upEntry.callback.AddListener((_) => StartCoroutine(SendCommand((int)CarCmd.Stop)));
        trigger.triggers.Add(upEntry);
    }

    IEnumerator SendCommand(int val)
    {
        using var req = UnityWebRequest.Get($"{baseUrl}/control?var=car&val={val}");
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            Debug.LogError($"CarCmd {val} failed: {req.error}");
    }
}
