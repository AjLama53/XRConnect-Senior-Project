using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ESP32CameraFeed : MonoBehaviour
{
    [Tooltip("Drag your RawImage here")]
    public RawImage cameraView;

    private string camUrl => $"{AppSettingsManager.Instance.carBaseUrl}/capture";

    void Start()
    {
        StartCoroutine(StreamLoop());
    }

    IEnumerator StreamLoop()
    {
        while (true)
        {
            using (var req = UnityWebRequestTexture.GetTexture(camUrl))
            {
                yield return req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.Success)
                {
                    cameraView.texture = ((DownloadHandlerTexture)req.downloadHandler).texture;
                }
                else
                {
                    Debug.LogError($"Stream error: {req.error}");
                }
            }
            yield return new WaitForSeconds(0.1f); // ~10 FPS
        }
    }
}
