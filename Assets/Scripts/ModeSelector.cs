using UnityEngine;

public class ModeSelector : MonoBehaviour
{
    private MonoBehaviour[] mode0Scripts;
    private MonoBehaviour[] mode1Scripts;
    private MonoBehaviour[] mode2Scripts;

    void Awake()
    {
        // Automatically find all delete scripts in the scene
        mode0Scripts = FindObjectsOfType<TileRemover>(true);
        mode1Scripts = FindObjectsOfType<TilePlusRemover>(true);
        mode2Scripts = FindObjectsOfType<Tile2DownRemover>(true);

        Debug.Log("Auto-detected scripts:");
        Debug.Log("Mode0 count: " + mode0Scripts.Length);
        Debug.Log("Mode1 count: " + mode1Scripts.Length);
        Debug.Log("Mode2 count: " + mode2Scripts.Length);

        // Default starting mode
        SetMode(0);
    }

    public void SetMode(int index)
    {
        EnableArray(mode0Scripts, index == 0);
        EnableArray(mode1Scripts, index == 1);
        EnableArray(mode2Scripts, index == 2);

        Debug.Log("Switched to delete mode: " + index);
    }

    private void EnableArray(MonoBehaviour[] scripts, bool enable)
    {
        foreach (var script in scripts)
        {
            if (script != null)
                script.enabled = enable;
        }
    }
}
