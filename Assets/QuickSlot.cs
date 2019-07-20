using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : Slot
{
    public static int nowChild = 1;

    public override void SetChildNum(int num)
    {
        nowChild = num;
    }

    public override void UpdateInfo(bool isSlot, Sprite sprite)
    {
        base.UpdateInfo(isSlot, sprite);
        ItemIO.SaveQuickSlots(nowChild);
    }
}
