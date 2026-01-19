using UnityEngine;

public class ToolSelectButton : MonoBehaviour
{
    public ToolType toolType;

    public void SelectTool()
    {
        ToolManager.Instance.SelectTool(toolType);
    }
}
