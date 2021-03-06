﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {
    private static DataManager _dataManager = null;
    public SelectPlayer selectPlayer;

    /* 오브젝트 아이템 리스트 */
    private List<Dictionary<string, object>> itemList;
    public List<Item> items;
    public Sprite[] sprites;
    public GameObject[] materials;
    public Sprite defaultImg;

    /* 조합 레시피 리스트 */
    private List<Dictionary<string, object>> recipeList;

    /* 사냥 가능 오브젝트 리스트 */
    private List<Dictionary<string, object>> huntableList;

    public GameObject[] children;

    public static DataManager dataManager
    {
        get
        {
            if (_dataManager == null)
            {
                _dataManager = FindObjectOfType(typeof(DataManager)) as DataManager;

                if (_dataManager == null)
                {
                    Debug.Log("DataManager 없어요");
                }
            }

            return _dataManager;
        }
    }

    public GameObject[] GetChildren() // player 오브젝트 배열 리턴
    {
        return children;
    }


    private void Awake() // csv, xml 로드
    {
        selectPlayer = GetComponent<SelectPlayer>();

        /* 오브젝트 아이템 리스트 로드 */
        itemList = CSVReader.Read("Data/itemList");
        recipeList = CSVReader.Read("Data/RecipeList");
        huntableList = CSVReader.Read("Data/huntableTable");

        items = new List<Item>();

        for (int i = 0; i < itemList.Count; i++)
        {
            Item item = new Item();
            item.name = itemList[i]["name"].ToString();
            item.ID = itemList[i]["ID"].ToString();
            item.effect = itemList[i]["effect"].ToString();
            item.tool = itemList[i]["tool"].ToString();
            item.sprite = findSprite(item.ID);

            switch (itemList[i]["type"].ToString())
            {
                case "food":
                    item.type = Item.ItemType.food;
                    break;
                case "material":
                    item.type = Item.ItemType.material;
                    break;
                case "memo":
                    item.type = Item.ItemType.memo;
                    break;
                case "tool":
                    item.type = Item.ItemType.tool;
                    break;
            }

            items.Add(item);
        }
        /* 오브젝트 아이템 리스트 로드 끝 */
    }

    /* 아이템 리스트 Get 함수 */

    public List<Item> GetItems() // 아이템 리스트 리턴
    {
        return items;
    }

    public Item GetItem(string id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == id)
            {
                return items[i];
            }
        }

        return null;

    }


    public string getName(string id)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i]["ID"].ToString() == id)
            {
                return System.Convert.ToString(itemList[i]["name"].ToString());
            }
        }

        return "";
    }
    /*
    public int isGot(string id) // 아이템 분석 혹은 사용한 적 있는지 확인
    {
        
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i]["ID"].ToString() == id)
            {
                return System.Convert.ToInt32(itemList[i]["isGot"].ToString());
            }
        }
        

        ItemIO.isItemGot(id);

        return 0;
    }
    */

        /*
    public void GotItem(string id) // 아이템 사용 혹은 분석 시
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i]["ID"].ToString() == id)
            {
                itemList[i]["isGot"] = 1;
                return;
            }
        }
    }
        */

    public Sprite findSprite(string id) // 아이템 ID에 해당하는 스프라이트 리턴
    {
        foreach (Sprite spr in sprites)
        {
            if (spr.name == id)
            {
                return spr;
            }
        }

        return defaultImg;
    }

    public GameObject GetObj(string id)
    {
        foreach (GameObject mat in materials)
        {
            if (mat.name == id)
            {
                return mat;
            }
        }

        return null;
    }

    public float GetHP(string ID) // 아이템 효과 중 HP 값 리턴
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i]["ID"].ToString() == ID)
            {
                return System.Convert.ToSingle(itemList[i]["hp"].ToString());
            }
        }

        return -1;
    }

    public float GetFull(string ID) // 아이템 효과 중 포만감 값 리턴
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i]["ID"].ToString() == ID)
            {
                return System.Convert.ToSingle(itemList[i]["full"].ToString());
            }
        }

        return -1;
    }


    /* 아이템 리스트 Get 함수 끝 */

    /* 레시피 리스트 함수 */

    public List<Dictionary<string, object>> GetRecipeList()
    {
        return recipeList;
    }

    public Dictionary<string, object> GetRecipe(string id)
    {
        Dictionary<string, object> recipe = null;

        for (int i = 0; i < recipeList.Count; i++)
        {
            if (System.Convert.ToString(recipeList[i]["comID"]) == id)
            {
                recipe = recipeList[i];
            }
        }

        return recipe;
    }

    /* 레시피 리스트 함수 끝 */


    /* 스크립트 리스트 함수 */

    // 메모 스크립트 텍스트 가져오기

    /*  메모 스크립트 테이블 필요
    public string GetText(string id, int context)
    {
        string txt;

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i]["ID"].ToString() == id)
            {
                return System.Convert.ToSingle(itemList[i]["full"].ToString());
            }
        }

        return txt;
    }
    */

    /* 사냥 가능 오브젝트 리스트 함수 */
    public Dictionary<string, object> GetHuntable(string id)
    {
        Dictionary<string, object> huntable = null;

        for (int i = 0; i < huntableList.Count; i++)
        {
            Debug.Log(System.Convert.ToString(huntableList[i]["ID"]));
            if (System.Convert.ToString(huntableList[i]["ID"]) == id)
            {
                huntable = huntableList[i];
            }
        }

        return huntable;
    }
}
