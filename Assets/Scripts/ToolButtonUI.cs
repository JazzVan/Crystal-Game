using UnityEngine;
using UnityEngine.UI;

public class ToolButtonUI : MonoBehaviour
{
    public ToolType toolType;
    public Color brokenColor = Color.red;

    private Button button;
    private Image image;

    void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    void Update()
    {
        ToolData tool = ToolManager.Instance.GetToolData(toolType);
        if (tool == null)
            return;

        if (tool.isBroken)
        {
            button.interactable = false;
            image.color = brokenColor;
        }
    }
}
