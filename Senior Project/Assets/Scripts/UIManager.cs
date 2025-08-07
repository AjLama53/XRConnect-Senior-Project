using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject lightsMenu;

    private bool isMainMenuVisible = false;
    private bool isLightsMenuVisible = false;

    public void ToggleMainMenu()
    {
        isMainMenuVisible = !isMainMenuVisible;
        mainMenu.SetActive(isMainMenuVisible);

        // Hide lights menu if main is closed
        if (!isMainMenuVisible)
        {
            isLightsMenuVisible = false;
            lightsMenu.SetActive(false);
        }
    }

    public void ToggleLightsMenu()
    {
        isLightsMenuVisible = !isLightsMenuVisible;
        lightsMenu.SetActive(isLightsMenuVisible);
    }
}
