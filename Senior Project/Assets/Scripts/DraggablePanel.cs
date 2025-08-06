using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DraggablePanel : MonoBehaviour
{

    public void ClosePanel()
    {
        Destroy(gameObject);
    }
}
