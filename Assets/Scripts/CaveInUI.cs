using TMPro;
using UnityEngine;

public class CaveInUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Update()
    {
        text.text = "Stability: " +
            CaveInManager.Instance.GetCurrentThreshold();
    }
}
