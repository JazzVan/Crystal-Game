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
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGem(GemType type)
    {
        totalGems++;

        if (!collectedGems.ContainsKey(type))
            collectedGems[type] = 0;

        collectedGems[type]++;

        SaveInventory();

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

    public string GetGemSummary()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (var kvp in collectedGems)
        {
            sb.AppendLine($"{kvp.Key}: {kvp.Value}");
        }

        return sb.ToString();
    }

    public void ResetInventory()
    {
        collectedGems.Clear();
    }

    void SaveInventory()
    {
        foreach (var gem in collectedGems)
        {
            PlayerPrefs.SetInt(gem.Key.ToString(), gem.Value);
        }

        PlayerPrefs.Save();
    }

    void LoadInventory()
    {
        foreach (GemType type in System.Enum.GetValues(typeof(GemType)))
        {
            int count = PlayerPrefs.GetInt(type.ToString(), 0);

            if (count > 0)
                collectedGems[type] = count;
        }
    }

}
