using UnityEngine;

public class CaveInManager : MonoBehaviour
{
    public static CaveInManager Instance;

    [Header("Cave In Settings")]
    public int startingThreshold = 5;

    private int currentThreshold;

    [Header("UI")]
    public GameObject caveInPanel;   // Drag your UI panel here

    public bool GameOver { get; private set; }  // <- NEW

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        currentThreshold = startingThreshold;
        GameOver = false;

        if (caveInPanel != null)
            caveInPanel.SetActive(false);
    }

    public void HitFracture(int damage)
    {
        if (GameOver)
            return;

        currentThreshold -= damage;

        Debug.Log($"Cave stability: {currentThreshold}");

        if (currentThreshold <= 0)
        {
            currentThreshold = 0;
            TriggerCaveIn();
        }
    }

    void TriggerCaveIn()
    {
        Debug.Log("CAVE IN!");

        GameOver = true;

        // Pause game
        Time.timeScale = 0f;

        // Show UI
        if (caveInPanel != null)
            caveInPanel.SetActive(true);
    }

    public int GetCurrentThreshold()
    {
        return currentThreshold;
    }
}
