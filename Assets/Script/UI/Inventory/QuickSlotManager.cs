using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    public Item[] quickSlotItems;
    public QuickSlot[] quickSlots;
    private int quickSlotCapacity = 4;
    public int moveChild;

    private void Awake()
    {
        quickSlotItems = ItemIO.LoadQuickSlotData();
        quickSlots = GetComponentsInChildren<QuickSlot>();

        moveChild = 1;

        SetQuickSlotItem();
    }

    public void SetMoveChild(int moveChild)
    {
        SaveQuickSlot();

        this.moveChild = moveChild;

        //EmptyAllQuickSlots();

        SetQuickSlotItem();
    }

    public void SetQuickSlotItem()
    {
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].AddItem(quickSlotItems[(moveChild - 1) * quickSlotCapacity + i]);
        }
    }

    public void SaveQuickSlot()
    {
        UpdateQuickSlotItems();
        ItemIO.SaveQuickSlots(quickSlotItems);
    }

    public void UpdateQuickSlotItems()
    {
        for (int i = 0; i < quickSlotCapacity; i++)
        {
            quickSlotItems[(moveChild - 1) * quickSlotCapacity + i] = quickSlots[i].ItemReturn();
        }
    }


    private void EmptyAllQuickSlots()
    {
        foreach(QuickSlot slot in quickSlots)
        {
            slot.item = null;
        }
    }

    private void OnApplicationQuit()
    {
        SaveQuickSlot();
    }
}
