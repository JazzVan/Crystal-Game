using UnityEngine;

public class VoluntaryEnd : MonoBehaviour
{
    public void QuitGame()
    {
        foreach (var entry in GemInventory.Instance.GetAllGems())
        {
            Debug.Log($"{entry.Key}: {entry.Value}");
        }

        Debug.Log("Quit button pressed");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
