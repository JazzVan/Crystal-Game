using TMPro;
using UnityEngine;

public class GemCounterUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Update()
    {
        text.text = $"Gems: {GemCounter.Instance.gemsCollected}";
    }
}
