using UnityEngine;

public class LightPanelManager : MonoBehaviour
{
    public GameObject light1PanelPrefab;
    public GameObject light2PanelPrefab;

    private GameObject light1PanelInstance;
    private GameObject light2PanelInstance;

    private Vector3 light1Offset = new Vector3(-0.3f, 0f, 1f);  // Relative to user
    private Vector3 light2Offset = new Vector3(0.3f, 0f, 1f);   // Offset right

    public Transform xrCamera;  // OVRCameraRig → CenterEyeAnchor

    private void Start()
    {
        // Auto-assign if not set
        if (xrCamera == null)
        {
            GameObject rig = GameObject.Find("OVRCameraRig");
            if (rig != null)
            {
                Transform centerEye = rig.transform.Find("TrackingSpace/CenterEyeAnchor");
                if (centerEye != null)
                {
                    xrCamera = centerEye;
                }
                else
                {
                    Debug.LogWarning("⚠️ Could not find CenterEyeAnchor inside OVRCameraRig.");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ Could not find OVRCameraRig in scene.");
            }
        }
    }

    public void ToggleLight1Panel()
    {
        if (light1PanelInstance == null)
        {
            // Try to find it in the scene if it was left open
            light1PanelInstance = GameObject.Find("Light1Panel(Clone)");
        }

        if (light1PanelInstance == null)
        {
            Vector3 spawnPos = xrCamera.position + xrCamera.TransformDirection(light1Offset);
            Quaternion rot = Quaternion.LookRotation(spawnPos - xrCamera.position);
            light1PanelInstance = Instantiate(light1PanelPrefab, spawnPos, rot);
            light1PanelInstance.name = "Light1Panel(Clone)";
        }
        else
        {
            Destroy(light1PanelInstance);
            light1PanelInstance = null;
        }
    }

    public void ToggleLight2Panel()
    {
        if (light2PanelInstance == null)
        {
            light2PanelInstance = GameObject.Find("Light2Panel(Clone)");
        }

        if (light2PanelInstance == null)
        {
            Vector3 spawnPos = xrCamera.position + xrCamera.TransformDirection(light2Offset);
            Quaternion rot = Quaternion.LookRotation(spawnPos - xrCamera.position);
            light2PanelInstance = Instantiate(light2PanelPrefab, spawnPos, rot);
            light2PanelInstance.name = "Light2Panel(Clone)";
        }
        else
        {
            Destroy(light2PanelInstance);
            light2PanelInstance = null;
        }
    }

}
