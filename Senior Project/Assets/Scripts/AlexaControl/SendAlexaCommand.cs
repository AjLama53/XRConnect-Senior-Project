using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SendAlexaCommand : MonoBehaviour
{
    // Replace with your actual API Gateway endpoint URL.
    public string apiUrl = "https://o1fbagark1.execute-api.us-east-1.amazonaws.com/vr-control";

    // This method is called by your VR button.
    // To trigger music playback, call SendCommand("PlayMusicIntent");
    public void SendCommand(string intentName)
    {
        StartCoroutine(SendCommandToAlexa(intentName));
    }

    private IEnumerator SendCommandToAlexa(string intentName)
    {
        // Construct the JSON payload. It must match the Alexa Skill request model.
        string jsonData = @"{
  ""session"": {
    ""new"": true,
    ""sessionId"": ""SessionId.1234567890"",
    ""application"": { ""applicationId"": ""amzn1.ask.skill.c51c9d73-7230-455f-8ef2-1e73bf7b4805"" },
    ""attributes"": {},
    ""user"": { ""userId"": ""amzn1.ask.account.someUserId"" }
  },
  ""context"": {
    ""System"": {
      ""device"": { ""deviceId"": ""device123"", ""supportedInterfaces"": {} },
      ""application"": { ""applicationId"": ""amzn1.ask.skill.c51c9d73-7230-455f-8ef2-1e73bf7b4805"" },
      ""user"": { ""userId"": ""amzn1.ask.account.someUserId"" }
    }
  },
  ""request"": {
    ""type"": ""IntentRequest"",
    ""requestId"": ""EdwRequestId.1234567890"",
    ""timestamp"": ""2025-04-08T00:00:00Z"",
    ""locale"": ""en-US"",
    ""intent"": {
      ""name"": """ + intentName.Trim() + @""",
      ""confirmationStatus"": ""NONE"",
      ""slots"": {}
    }
  },
  ""version"": ""1.0""
}";
        Debug.Log("Sending JSON to Alexa: " + jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Alexa Command Sent: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error sending command: " + request.error);
        }
    }
}
