using UnityEngine;

public class ToolDurabilityManager : MonoBehaviour
{
    public static ToolDurabilityManager Instance;

    private int totalTools;
    private int brokenTools;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterTool()
    {
        totalTools++;
    }

    public void NotifyToolBroken()
    {
        brokenTools++;

        Debug.Log($"Tool broken ({brokenTools}/{totalTools})");

        if (brokenTools >= totalTools)
        {
            QuitGame();
        }
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
