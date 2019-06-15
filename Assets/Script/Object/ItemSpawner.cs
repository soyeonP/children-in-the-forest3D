using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    private List<Item> items;
    public Sprite defaultImg;

    public GameObject objPrefab;
    public GameObject objParent;

    private DataManager dm;

    private void Start()
    {
        dm = DataManager.dataManager;
        items = dm.GetItems();


        for (int i = 0; i < 20; i++)
        {
            Item item = items[Random.Range(0, items.Count)];

            GameObject obj = SpawnObj(item);

            /* 오브젝트 생성 - 테스트용으로 화면 안에서만 */
            float yy = obj.GetComponent<BoxCollider>().size.y;
            obj.transform.position = FindSpawnPosition(yy);

            obj.name = item.ID;
        }
    }

    public GameObject SpawnObj(Item item) 
    {
        Debug.Log(item.ID);
        GameObject obj = Instantiate(dm.GetObj(item.ID), objParent.transform);
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
