using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    private List<Item> items;
    public Sprite defaultImg;

    public GameObject objPrefab;
    public GameObject objParent;

    private DataManager dm;

    private void Start()
    {
        dm = DataManager.dataManager;
        items = dm.GetItems();

        /* 수동 배치 오브젝트 설정 */
        for (int i = 0; i < objParent.transform.childCount; i++)
        {
            GameObject obj = objParent.transform.GetChild(i).gameObject;
            Item item = DataManager.dataManager.GetItem(obj.name);

            obj.AddComponent<Item>();
            obj.AddComponent<BoxCollider>();

            if (item.type != Item.ItemType.memo)
            {
                Vector3 oldPos = obj.transform.position;
                oldPos.y = obj.GetComponent<BoxCollider>().size.y / 2 * obj.transform.localScale.y;

                obj.transform.position = oldPos;

                obj.tag = "Object";
            }
            if (obj.name == "rabbit")
            {
                obj.tag = "Huntable";
                obj.AddComponent<Huntable>();
            }

            Item itemCompo = obj.GetComponent<Item>();
            itemCompo.name = item.name;
            itemCompo.ID = item.ID;
            itemCompo.tool = item.tool;
            itemCompo.sprite = item.sprite;
            itemCompo.type = item.type;
            itemCompo.effect = item.effect;
        }

        /* 랜덤 스폰 설정
        for (int i = 0; i < 20; i++)
        {
            Item item = items[Random.Range(0, items.Count)];

            GameObject obj = SpawnObj(item);

            / 오브젝트 생성 - 테스트용으로 화면 안에서만 /
            if (obj != null)
            {
                float yy = obj.GetComponent<BoxCollider>().size.y;
                obj.transform.position = FindSpawnPosition(yy);

                obj.name = item.ID;
            }
        }
        */

    }

    public GameObject SpawnObj(Item item)
    {
        GameObject objPrefab = dm.GetObj(item.ID);
        if (objPrefab != null)
        {
            Debug.Log(objPrefab.name);

            GameObject obj = Instantiate(objPrefab, objParent.transform);
            obj.AddComponent<Item>();
            obj.AddComponent<BoxCollider>();
            obj.tag = "Object";
            Item itemCompo = obj.GetComponent<Item>();

            itemCompo.name = item.name;
            itemCompo.ID = item.ID;
            itemCompo.tool = item.tool;
            itemCompo.sprite = item.sprite;
            itemCompo.type = item.type;
            itemCompo.effect = item.effect;

            return obj;
        }
        else return null;
    }

    private Vector3 FindSpawnPosition(float yy)
    {
        Vector3 position;
        RaycastHit hit;
        do
        {
            position = new Vector3(Random.Range(-45f, 45f), yy / 2, Random.Range(-45f, 45f));
            Ray ray = Camera.main.ScreenPointToRay(position);
            Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity);

        } while (hit.collider.tag == "terrain");

        return position;
    }
}
