using UnityEngine;
using System.Collections.Generic;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance;

    public List<ToolData> tools = new List<ToolData>();
    public ToolType activeTool;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        foreach (var tool in tools)
            tool.Init();
    }

    public bool CanUseActiveTool()
    {
        ToolData tool = tools.Find(t => t.toolType == activeTool);
        return tool != null && !tool.isBroken;
    }

    public void ConsumeActiveToolUse()
    {
        ToolData tool = GetToolData(activeTool);
        if (tool == null || tool.isBroken)
            return;

        tool.Use();

        Debug.Log($"{activeTool} uses left: {tool.currentUses}");

        if (tool.isBroken)
        {
            Debug.Log($"{activeTool} BROKEN");
        }

        if (AllToolsBroken())
        {
            QuitGame();
        }
    }



    public bool IsToolBroken(ToolType type)
    {
        return tools.Find(t => t.toolType == type)?.isBroken ?? true;
    }

    public void SelectTool(ToolType type)
    {
        if (!IsToolBroken(type))
            activeTool = type;
        Debug.Log("Tool Selected: " + type);
    }

    public ToolData GetToolData(ToolType type)
    {
        return tools.Find(t => t.toolType == type);
    }

    bool AllToolsBroken()
    {
        foreach (var tool in tools)
        {
            if (!tool.isBroken)
                return false;
        }
        return true;
    }

    void QuitGame()
    {
        foreach (var entry in GemInventory.Instance.GetAllGems())
        {
            Debug.Log($"{entry.Key}: {entry.Value}");
        }
        Debug.Log("All tools broken. Quitting game.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
