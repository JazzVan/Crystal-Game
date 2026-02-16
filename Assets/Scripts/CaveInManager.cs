using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class CaveInManager : MonoBehaviour
{
    public static CaveInManager Instance;

    [Header("Cave In Settings")]
    public int startingThreshold = 5;

    private int currentThreshold;

    [Header("UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gemSummaryText;

    public bool GameOver { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        ResetThreshold();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void ResetThreshold()
    {
        currentThreshold = startingThreshold;
        GameOver = false;
        Time.timeScale = 1f;
    }

    public void HitFracture(int damage)
    {
        if (GameOver)
            return;

        currentThreshold -= damage;

        if (currentThreshold <= 0)
        {
            currentThreshold = 0;
            TriggerCaveIn();
        }
    }

    void TriggerCaveIn()
    {
        GameOver = true;

        Time.timeScale = 0f;

        ShowGemSummary();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    void ShowGemSummary()
    {
        if (gemSummaryText == null)
            return;

        var summary = GemInventory.Instance.GetGemSummary();

        gemSummaryText.text = summary;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public int GetCurrentThreshold()
    {
        return currentThreshold;
    }
}
