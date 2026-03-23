using UnityEngine;
using UnityEngine.UI;

public class ToolSelectionButton : MonoBehaviour
{
    public ToolType toolType;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            image.color = new Color32(0, 255, 0, 255); // Green
        }
        else
        {
            image.color = Color.white;
        }
    }
}
