using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameOverReason
{
    CaveIn,
    OutOfTools
}


public class CaveInManager : MonoBehaviour
{
    public static CaveInManager Instance;

    [Header("Cave In Settings")]
    public int startingThreshold = 5;

    private int currentThreshold;

    [Header("UI")]
    public TextMeshProUGUI gameOverReasonText;
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

    public void TriggerGameOver()
    {
        if (GameOver)
            return;

        GameOver = true;

        Time.timeScale = 0f;

        ShowGemSummary();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void TriggerGameOver(GameOverReason reason)
    {
        if (GameOver)
            return;

        GameOver = true;

        Time.timeScale = 0f;

        ShowGemSummary();
        ShowGameOverReason(reason);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }


    void TriggerCaveIn()
    {
        TriggerGameOver(GameOverReason.CaveIn);

    }

    void ShowGemSummary()
    {
        if (gemSummaryText == null)
            return;

        var summary = GemInventory.Instance.GetGemSummary();

        gemSummaryText.text = summary;
    }

    void ShowGameOverReason(GameOverReason reason)
    {
        if (gameOverReasonText == null)
            return;

        switch (reason)
        {
            case GameOverReason.CaveIn:
                gameOverReasonText.text = "CAVE IN!";
                break;

            case GameOverReason.OutOfTools:
                gameOverReasonText.text = "OUT OF TOOLS!";
                break;
        }
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
