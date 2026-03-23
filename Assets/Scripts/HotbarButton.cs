using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI label;
    public Image image;

    private ToolType toolType;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Setup(ToolType tool)
    {
        toolType = tool;

        label.text = tool.ToString();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            ToolManager.Instance.SelectTool(toolType);
        });
    }

    void Update()
    {
        var toolData = ToolManager.Instance.GetToolData(toolType);

        if (toolData == null)
            return;

        if (toolData.isBroken)
        {
            image.color = new Color32(255, 0, 0, 255); // FF0000 (red)
            button.interactable = false;
        }
        else
        {
            image.color = new Color32(54, 54, 54, 255); // 363636 (dark gray)
            button.interactable = true;
        }
    }

}
