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

    public bool TryUseActiveTool()
    {
        ToolData tool = tools.Find(t => t.toolType == activeTool);

        if (tool == null || tool.isBroken)
            return false;

        tool.Use();

        if (AllToolsBroken())
        {
            QuitGame();
        }

        return true;
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
        Debug.Log("All tools broken. Quitting game.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
