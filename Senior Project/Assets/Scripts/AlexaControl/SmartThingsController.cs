using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Text;

public class SmartThingsController : MonoBehaviour
{
    // Personal Access Tokens

    public string LOCK_PAT;
    private const string SENSOR_PAT = "587dd98d-f9dd-4251-82cd-95da1e58973e";

    // Device IDs
    private const string LOCK_DEVICE_ID = "baca090d-1b90-4110-b1de-0a4a1511f89c";
    private const string SENSOR_DEVICE_ID = "8cdb2237-2d38-426a-bc75-02212a635f76";
    private const string PAUSE_DEVICE_ID = "42079461-9004-4fba-8528-96e41729b351";

    private string lockStateFile;
    private string sensorStateFile;

    void Start()
    {
        LOCK_PAT = AppSettingsManager.Instance.lockPAT;
        lockStateFile = Path.Combine(Application.persistentDataPath, "lockstate.txt");
        sensorStateFile = Path.Combine(Application.persistentDataPath, "sensorstate.txt");
    }

    // Trigger Lock/Unlock Toggle
    public void TriggerLockToggle()
    {
        StartCoroutine(RunLockToggle());
    }

    // Trigger Sensor State (toggle open/closed)
    public void TriggerSensorToggle()
    {
        StartCoroutine(RunSensorToggle());
    }

    // Trigger Pause Lock
    public void TriggerPauseLock()
    {
        StartCoroutine(RunPauseLock());
    }

    IEnumerator RunLockToggle()
    {
        string lastState = "unlock";
        if (File.Exists(lockStateFile))
            lastState = File.ReadAllText(lockStateFile).Trim().ToLower();

        string nextState = lastState == "lock" ? "unlock" : "lock";

        string url = $"https://api.smartthings.com/v1/devices/{LOCK_DEVICE_ID}/commands";
        yield return SendCommand(url, LOCK_PAT, new Command[] {
            new Command("main", "lock", nextState)
        }, success =>
        {
            if (success)
                File.WriteAllText(lockStateFile, nextState);
        });
    }

    IEnumerator RunSensorToggle()
    {
        string lastState = "closed";
        if (File.Exists(sensorStateFile))
            lastState = File.ReadAllText(sensorStateFile).Trim().ToLower();

        string nextState = lastState == "open" ? "closed" : "open";

        string url = $"https://api.smartthings.com/v1/devices/{SENSOR_DEVICE_ID}/commands";
        yield return SendCommand(url, SENSOR_PAT, new Command[] {
            new Command("main", "contactSensor", nextState)
        }, success =>
        {
            if (success)
                File.WriteAllText(sensorStateFile, nextState);
        });
    }

    IEnumerator RunPauseLock()
    {
        string url = $"https://api.smartthings.com/v1/devices/{PAUSE_DEVICE_ID}/commands";
        yield return SendCommand(url, LOCK_PAT, new Command[] {
            new Command("main", "lock", "lock")
        }, null);
    }

    IEnumerator SendCommand(string url, string pat, Command[] commands, System.Action<bool> callback)
    {
        CommandWrapper payload = new CommandWrapper { commands = commands };
        string jsonBody = JsonUtility.ToJson(payload);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody));
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Authorization", $"Bearer {pat}");
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("📡 SmartThings Response: " + request.downloadHandler.text);
            callback?.Invoke(true);
        }
        else
        {
            Debug.LogError($"❌ SmartThings Error: {request.responseCode} - {request.error}\n{request.downloadHandler.text}");
            callback?.Invoke(false);
        }
    }

    [System.Serializable]
    public class Command
    {
        public string component;
        public string capability;
        public string command;

        public Command(string component, string capability, string command)
        {
            this.component = component;
            this.capability = capability;
            this.command = command;
        }
    }

    [System.Serializable]
    public class CommandWrapper
    {
        public Command[] commands;
    }
}
