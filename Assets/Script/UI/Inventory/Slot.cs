using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

    public Item item;               // 해당 슬롯이 가진 아이템
    public Sprite DefaultImg;       // 빈 슬롯 이미지

    public Image ItemImg;           // 0번 child의 image (item image 보여줌)
    protected bool isSlot = false;    // 슬롯 비었으면 false, 차 있으면 true

    protected int childNum;
    public GameObject[] Children;

    public virtual void SetChildNum(int num) { childNum = num; }
    public Item ItemReturn() { return this.item; }
    public bool isSlots() { return isSlot; } // 차있으면 true
    public void SetSlots(bool isSlot) { this.isSlot = isSlot; } // isSlot -> 비면 false

    public virtual void AddItem(Item item)
    {
        this.item = item;
        if (item != null)
            UpdateInfo(true, item.sprite);
        else UpdateInfo(false, DefaultImg);
    }

    public void emptyItem()
    {
        if (!isSlot)
        {
            return;
        }
        this.item = null;

        UpdateInfo(false, DefaultImg);
        return;
    }

    public virtual void UpdateInfo(bool isSlot, Sprite sprite)
    {
        // 인벤토리 저장 및 그림자 설정

        SetSlots(isSlot);
        ItemImg.sprite = sprite;

        if (item != null && !ItemIO.isItemGot(item.ID)) 
        {
            ItemImg.color = Color.black;
        }
        else if (item == null)
        {
            ItemImg.color = Color.white;
        }

        ItemIO.SaveData();
    }

    public void ShowInfo()
    {
        ObjManager.objManager.inventory.RenewInfo(this);

        if (!ItemIO.isItemGot(item.ID))
        {
            ItemImg.color = Color.black;
        }
    }

    public void CheckItem() // 아이템 조사 시
    {
        ItemIO.GotItemSave(item.ID);
        ObjManager.objManager.inventory.RenewInfo(this);
        ItemImg.color = Color.white;

        ObjManager.objManager.inventory.RenewInventory();
    }

    public virtual void UseItem() // 아이템 사용 시
    {
        // 해당 아이템 가지고 있는 캐릭터 가져와 상태값 변경
        DataManager dm = DataManager.dataManager;

        childNum = GetChildNum();

        switch (item.type)
        {
            case Item.ItemType.food:
                Children = dm.GetChildren();
                playerState character = Children[childNum].GetComponent<playerState>();
                character.ChangeFull(dm.GetFull(item.ID));
                character.ChangeHP(dm.GetHP(item.ID));
                ItemIO.GotItemSave(item.ID);
                emptyItem();
                break;

            case Item.ItemType.memo:
                // 메모 패널 열기
                // 메모 글 불러오기
                ObjManager.objManager.inventory.OpenMemo(item.ID);
                break;

            case Item.ItemType.trap:
                ItemSpawner spawner = dm.gameObject.GetComponent<ItemSpawner>();
                GameObject obj = spawner.SpawnObj(item);

                // 오브젝트 위치 설정
                Vector3 charPos = dm.GetChildren()[childNum - 1].transform.position;
                obj.transform.position = new Vector3(charPos.x, obj.GetComponent<BoxCollider>().size.y / 2, charPos.z);

                // 오브젝트 이름, 태그 설정
                obj.name = item.ID;
                obj.tag = "trap";

                emptyItem();

                break;
        }

        /* 인벤토리 새로고침 */
        ObjManager.objManager.inventory.RenewInfo();
        ObjManager.objManager.inventory.RenewInventory();
    }

    protected virtual int GetChildNum()
    {
        return System.Convert.ToInt32(gameObject.name) / 6;
    }

    public void ThrowItem() // 아이템 버릴 시
    {
        emptyItem();
        ObjManager.objManager.inventory.RenewInfo();
        ObjManager.objManager.inventory.RenewInventory();
    }

    public void CheckGotItem()
    {
        if (item != null)
        {
            if (ItemIO.isItemGot(item.ID))
                ItemImg.color = Color.white;
            else ItemImg.color = Color.black;
        }
    }
}
