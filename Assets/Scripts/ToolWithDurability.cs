using UnityEngine;

public abstract class ToolWithDurability : MonoBehaviour
{
    [SerializeField] protected int maxUses = 5;

    protected int currentUses;
    protected bool isBroken = false;
    private bool initialized = false;

    protected virtual void Start()
    {
        if (initialized) return;

        initialized = true;
        currentUses = maxUses;

        if (ToolDurabilityManager.Instance != null)
        {
            ToolDurabilityManager.Instance.RegisterTool();
        }
        else
        {
            Debug.LogError("ToolDurabilityManager not found in scene!");
        }
    }

    protected bool CanUseTool()
    {
        return !isBroken && currentUses > 0;
    }

    protected void ConsumeUse()
    {
        if (isBroken) return;

        currentUses--;

        if (currentUses <= 0)
        {
            isBroken = true;
            DisableTool();

            ToolDurabilityManager.Instance?.NotifyToolBroken();
        }
    }

    protected virtual void DisableTool()
    {
        Debug.Log($"{gameObject.name} is broken!");
        enabled = false;
    }
}
