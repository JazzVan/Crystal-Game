using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ToolSelectionUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI selectedText;
    public ToolHotbarUI hotbarUI;
    public List<ToolSelectionButton> toolButtons;

    private List<ToolType> selected = new List<ToolType>();

    void Start()
    {
        Time.timeScale = 0f;
    }

    public void ToggleTool(ToolType tool)
    {
        if (selected.Contains(tool))
        {
            selected.Remove(tool);
        }
        else
        {
            if (selected.Count >= ToolManager.Instance.maxToolsPerRun)
                return;

            selected.Add(tool);
        }

        UpdateUI();
    }

    public void TogglePickaxe()
    {
        ToggleTool(ToolType.Pickaxe);
    }

    public void ToggleHammer()
    {
        ToggleTool(ToolType.Hammer);
    }

    public void ToggleShovel()
    {
        ToggleTool(ToolType.Shovel);
    }

    public void ToggleHose()
    {
        ToggleTool(ToolType.HighPressureHose);
    }

    public void ToggleLaser()
    {
        ToggleTool(ToolType.PrecisionMiningLaser);
    }


    void UpdateUI()
    {
        selectedText.text = $"Selected: {selected.Count}/4";

        foreach (var button in toolButtons)
        {
            bool isSelected = selected.Contains(button.toolType);
            button.SetSelected(isSelected);
        }
    }


    public void StartRun()
    {
        if (selected.Count < ToolManager.Instance.maxToolsPerRun)
        {
            Debug.Log("Select 4 tools first!");
            return;
        }

        ToolManager.Instance.selectedTools = new List<ToolType>(selected);

        panel.SetActive(false);

        // Enable gameplay
        Time.timeScale = 1f;

        hotbarUI.SetupHotbar();
    }
}
