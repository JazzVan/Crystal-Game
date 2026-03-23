using UnityEngine;
using System.Collections.Generic;

public class ToolHotbarUI : MonoBehaviour
{
    public List<HotbarButton> hotbarButtons;
    //var data = ToolManager.Instance.GetToolData(tool);
    //labels[i].text = $"{tool} ({data.currentUses})";

    public void SetupHotbar()
    {
        var selected = ToolManager.Instance.selectedTools;

        for (int i = 0; i < hotbarButtons.Count; i++)
        {
            if (i < selected.Count)
            {
                hotbarButtons[i].gameObject.SetActive(true);
                hotbarButtons[i].Setup(selected[i]);
            }
            else
            {
                hotbarButtons[i].gameObject.SetActive(false);
            }
        }
    }
}