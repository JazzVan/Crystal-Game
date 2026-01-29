using UnityEngine;

public class GemCounter : MonoBehaviour
{
    public static GemCounter Instance;

    public int gemsCollected = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddGem(int amount = 1)
    {
        gemsCollected += amount;
        Debug.Log($"Gems: {gemsCollected}");
    }
}
