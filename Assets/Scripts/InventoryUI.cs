using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public GameObject inventoryPanel;
    public TextMeshProUGUI inventoryText;

    public bool IsOpen { get; private set; }

    void Awake()
    {
        Instance = this;
        inventoryPanel.SetActive(false);
    }

    public void ToggleInventory()
    {
        IsOpen = !IsOpen;

        inventoryPanel.SetActive(IsOpen);

        Time.timeScale = IsOpen ? 0f : 1f;

        if (IsOpen)
            Refresh();
    }

    void Refresh()
    {
        inventoryText.text = GemInventory.Instance.GetGemSummary();
    }
}
