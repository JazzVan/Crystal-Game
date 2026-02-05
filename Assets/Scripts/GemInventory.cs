using UnityEngine;
using System.Collections.Generic;

public class GemInventory : MonoBehaviour
{
    public static GemInventory Instance;

    public int totalGems = 0;

    private Dictionary<GemType, int> collectedGems =
        new Dictionary<GemType, int>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddGem(GemType type)
    {
        totalGems++;

        if (!collectedGems.ContainsKey(type))
            collectedGems[type] = 0;

        collectedGems[type]++;

        Debug.Log($"{type} collected. Total: {collectedGems[type]}");
    }

    public int GetGemCount(GemType type)
    {
        return collectedGems.ContainsKey(type)
            ? collectedGems[type]
            : 0;
    }

    public Dictionary<GemType, int> GetAllGems()
    {
        return collectedGems;
    }
}
