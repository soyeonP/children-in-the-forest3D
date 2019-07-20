using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : Slot
{
    private QuickSlotManager quickSlotManager;

    private void Awake()
    {
        quickSlotManager = GetComponentInParent<QuickSlotManager>();
    }

    public override void UpdateInfo(bool isSlot, Sprite sprite)
    {
        base.UpdateInfo(isSlot, sprite);
    }

    protected override int GetChildNum()
    {
        return quickSlotManager.moveChild;
    }
}
