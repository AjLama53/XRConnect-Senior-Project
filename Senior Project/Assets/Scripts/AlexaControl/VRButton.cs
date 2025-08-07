using UnityEngine;

public class VRButton : MonoBehaviour
{
    public SendAlexaCommand alexaCommand;

    public string commandToSend = "playMusic"; // Customize this per button

    private void OnMouseDown()
    {
        if (alexaCommand != null)
        {
            alexaCommand.SendCommand(commandToSend);
        }
        else
        {
            Debug.LogError("AlexaCommand script not assigned.");
        }
    }
}
