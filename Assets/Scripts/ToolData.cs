using UnityEngine;

[System.Serializable]
public class ToolData
{
    public ToolType toolType;
    public int maxUses = 5;
    [HideInInspector] public int currentUses;
    [HideInInspector] public bool isBroken;

    public void Init()
    {
        currentUses = maxUses;
        isBroken = false;
    }

    public bool Use()
    {
        if (isBroken) return false;

        currentUses--;

        if (currentUses <= 0)
        {
            isBroken = true;
        }

        return true;
    }
}
