using UnityEngine;

public class MenuToggleController : MonoBehaviour
{
    [Header("References")]
    public GameObject menuPrefab;             // Assign your world-space menu prefab here
    public Transform xrCamera;                // Usually OVRCameraRig → TrackingSpace → CenterEyeAnchor

    [Header("Spawn Settings")]
    [Range(0.3f, 1.5f)]
    public float menuSpawnDistance = 0.6f;
    public Vector3 menuVerticalOffset = new Vector3(0f, -0.05f, 0f); // slightly below eye level

    private GameObject activeMenuInstance;

    void Update()
    {
        // 🎮 Controller Button: A (Right) or X (Left)
        if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
        {
            ToggleMenu();
        }

        // 💻 Editor Testing: Press 'M' to toggle
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (activeMenuInstance == null)
        {
            ShowMenu();
        }
        else
        {
            Destroy(activeMenuInstance);
            activeMenuInstance = null;
        }
    }

    private void ShowMenu()
    {
        if (xrCamera == null)
        {
            Debug.LogWarning("❗ xrCamera is not assigned!");
            return;
        }

        // Calculate flat forward (keeps menu level with horizon)
        Vector3 flatForward = Vector3.ProjectOnPlane(xrCamera.forward, Vector3.up).normalized;
        Vector3 spawnPos = xrCamera.position + flatForward * menuSpawnDistance + menuVerticalOffset;
        Quaternion spawnRot = Quaternion.LookRotation(flatForward, Vector3.up);

        // Instantiate and rotate to face user
        activeMenuInstance = Instantiate(menuPrefab, spawnPos, spawnRot);
    }
}
