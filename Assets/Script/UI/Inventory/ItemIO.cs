﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public sealed class ItemIO : MonoBehaviour {

    public GameObject ObjMan;
    private static int quickSlotCapacity = 4;

    public static void GotItemSave (string id)
    {
        XmlDocument xmldoc = new XmlDocument();
        XmlElement child;

        if (System.IO.File.Exists(Application.dataPath + "/ItemGetData.xml"))
        { // 파일 존재할 경우
            xmldoc.Load(Application.dataPath + "/ItemGetData.xml");
            child = xmldoc["Items"];
        }
        else
        {
            child = xmldoc.CreateElement("Items");
            xmldoc.AppendChild(child);
        }

        XmlElement idData = xmldoc.CreateElement("Item");
        idData.SetAttribute("ID", id);

        child.AppendChild(idData);

        xmldoc.Save(Application.dataPath + "/ItemGetData.xml");
    }

    public static bool isItemGot (string id)
    {
        XmlDocument xmldoc = new XmlDocument();
        XmlElement child;

        if (System.IO.File.Exists(Application.dataPath + "/ItemGetData.xml"))
        { // 파일 존재할 경우
            xmldoc.Load(Application.dataPath + "/ItemGetData.xml");
            child = xmldoc["Items"];
        }
        else
        {
            return false;
        }

        foreach (XmlElement itemElement in child.ChildNodes)
        {
            if (itemElement.GetAttribute("ID").ToString() == id)
                return true;
        }

        return false;
    }

    public static void SaveQuickSlots(Item[] quickSlotItems)
    {
        List<GameObject> quickSlots = ObjManager.objManager.inventory.quickSlots;

        XmlDocument XmlDoc = new XmlDocument();

        XmlElement child = XmlDoc.CreateElement("child");

        for (int i = 0; i < quickSlotItems.Length; i++)
        {
            //QuickSlot slot = quickSlots[i * quickSlotCapacity + k].GetComponent<QuickSlot>();
            Item item = quickSlotItems[i];

            if (item == null) continue;

            XmlElement setting = XmlDoc.CreateElement("Item");

            setting.SetAttribute("name", item.name);
            setting.SetAttribute("ID", item.ID);
            setting.SetAttribute("tool", item.tool.ToString());
            setting.SetAttribute("effect", item.effect);
            setting.SetAttribute("slotNum", i.ToString());

            switch (item.type)
            {
                case Item.ItemType.food:
                    setting.SetAttribute("type", "food");
                    break;
                case Item.ItemType.material:
                    setting.SetAttribute("type", "material");
                    break;
                case Item.ItemType.memo:
                    setting.SetAttribute("type", "memo");
                    break;
                case Item.ItemType.tool:
                    setting.SetAttribute("type", "tool");
                    break;
                case Item.ItemType.trap:
                    setting.SetAttribute("type", "trap");
                    break;
            }

            child.AppendChild(setting);

        }

        XmlDoc.AppendChild(child);

        XmlDoc.Save(Application.dataPath + "/QuickSlotData.xml");
    }

    public static Item[] LoadQuickSlotData()
    {
        Item[] items = new Item[3 * quickSlotCapacity];

        if (!System.IO.File.Exists(Application.dataPath + "/QuickSlotData.xml"))
        {
            return items;
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/QuickSlotData.xml");

            XmlElement child = xmlDoc["child"];

        foreach (XmlElement itemElement in child.ChildNodes)
        {
            Item item = new Item();

            item.name = itemElement.GetAttribute("name");
            item.ID = itemElement.GetAttribute("ID");
            item.effect = itemElement.GetAttribute("effect");
            item.tool = itemElement.GetAttribute("tool");
            item.sprite = DataManager.dataManager.findSprite(item.ID);

            switch (itemElement.GetAttribute("type"))
            {
                case "food":
                    item.type = Item.ItemType.food;
                    break;

                case "tool":
                    item.type = Item.ItemType.tool;
                    break;

                case "material":
                    item.type = Item.ItemType.material;
                    break;

                case "memo":
                    item.type = Item.ItemType.memo;
                    break;

                case "trap":
                    item.type = Item.ItemType.trap;
                    break;
            }

            items[System.Convert.ToInt32(itemElement.GetAttribute("slotNum"))] = item;
        }
        return items;
    }


    public static void SaveData()
    {
        List<GameObject> slots = ObjManager.objManager.inventory.slots;

        XmlDocument XmlDoc = new XmlDocument();
        XmlElement child = XmlDoc.CreateElement("Child");
        XmlDoc.AppendChild(child);

        Slot slot;
        int i = 0;

        while (i < slots.Count)
        {
            if ((slot = slots[i].GetComponent<Slot>()).isSlots())
            {
                // i번 인덱스에 아이템 있을 동안
                Item item = slot.ItemReturn();
                XmlElement setting = XmlDoc.CreateElement("Item");

                setting.SetAttribute("name", item.name);
                setting.SetAttribute("ID", item.ID);
                setting.SetAttribute("tool", item.tool.ToString());
                setting.SetAttribute("effect", item.effect);
                setting.SetAttribute("slotNum", i.ToString());

                switch(item.type)
                {
                    case Item.ItemType.food:
                        setting.SetAttribute("type", "food");
                        break;
                    case Item.ItemType.material:
                        setting.SetAttribute("type", "material");
                        break;
                    case Item.ItemType.memo:
                        setting.SetAttribute("type", "memo");
                        break;
                    case Item.ItemType.tool:
                        setting.SetAttribute("type", "tool");
                        break;
                    case Item.ItemType.trap:
                        setting.SetAttribute("type", "trap");
                        break;
                }

                child.AppendChild(setting);
            }

            i++;
        }

        XmlDoc.Save(Application.dataPath + "/InventoryData.xml");
    }

    public static Item[] LoadData()
    {
        Item[] items = new Item[18];

        if (!System.IO.File.Exists(Application.dataPath + "/InventoryData.xml"))
        {
            return null;
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/InventoryData.xml");
        XmlElement child = xmlDoc["Child"];

        foreach (XmlElement itemElement in child.ChildNodes)
        {
            Item item = new Item();

            item.name = itemElement.GetAttribute("name");
            item.ID = itemElement.GetAttribute("ID");
            item.effect = itemElement.GetAttribute("effect");
            item.tool = itemElement.GetAttribute("tool");
            item.sprite = DataManager.dataManager.findSprite(item.ID);

            switch (itemElement.GetAttribute("type"))
            {
                case "food":
                    item.type = Item.ItemType.food;
                    break;

                case "tool":
                    item.type = Item.ItemType.tool;
                    break;

                case "material":
                    item.type = Item.ItemType.material;
                    break;

                case "memo":
                    item.type = Item.ItemType.memo;
                    break;

                case "trap":
                    item.type = Item.ItemType.trap;
                    break;

            }

            items[System.Convert.ToInt32(itemElement.GetAttribute("slotNum"))] = item;
        }

        return items;
    }
}
