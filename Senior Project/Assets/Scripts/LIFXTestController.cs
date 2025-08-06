using UnityEngine;

public class LIFXTestController : MonoBehaviour
{
    private LIFXLANController lifx;

    private void Start()
    {
        lifx = GameObject.Find("LIFXController")?.GetComponent<LIFXLANController>();

        if (lifx == null)
        {
            Debug.LogError("❗️ LIFXLANController not found! Make sure the GameObject is named 'LIFXController'.");
        }
        else
        {
            Debug.Log("✅ LIFXTestController Ready. Press keys to control the bulb.");
        }
    }


    private void Update()
    {
        // Test with keyboard inputs
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            lifx.SetPower(true);
            Debug.Log("🔆 Turned ON bulb.");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            lifx.SetPower(false);
            Debug.Log("💡 Turned OFF bulb.");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            lifx.SetColor(0, 65535, 32768, 3500);  // Red color, 50% brightness
            Debug.Log("🎨 Set bulb to RED.");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            lifx.SetColor(21845, 65535, 32768, 3500);  // Green color
            Debug.Log("🎨 Set bulb to GREEN.");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            lifx.SetColor(43690, 65535, 32768, 3500);  // Blue color
            Debug.Log("🎨 Set bulb to BLUE.");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            lifx.SendGetVersion();
            Debug.Log("ℹ️ Checked bulb version.");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            lifx.GetPower();
            Debug.Log("🔍 Checked bulb power status.");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            lifx.SendGetService();
            Debug.Log("📡 Sent GetService packet.");
        }
    }
}
