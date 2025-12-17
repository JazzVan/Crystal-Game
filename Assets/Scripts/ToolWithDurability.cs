using UnityEngine;

public abstract class ToolWithDurability : MonoBehaviour
{
    [SerializeField] protected int maxUses = 5;
    protected int currentUses;

    protected virtual void Awake()
    {
        currentUses = maxUses;
        ToolDurabilityManager.Instance.RegisterTool();
    }

    protected bool isBroken = false;

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
            ToolDurabilityManager.Instance.NotifyToolBroken();
        }
    }


    protected virtual void DisableTool()
    {
        Debug.Log($"{gameObject.name} is broken!");
        enabled = false; // disables THIS script
    }
}
