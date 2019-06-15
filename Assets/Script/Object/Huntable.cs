using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huntable : MonoBehaviour
{
    DataManager dm;
    private Item objData;
    private string id;
    public string killed;
    private string tool;
    private float hp;

    private void Start()
    {
        id = gameObject.name;

        dm = DataManager.dataManager;
        Dictionary<string, object> huntable = dm.GetHuntable(id);
        objData = gameObject.GetComponent<Item>();

        killed = huntable["killedID"].ToString();
        tool = huntable["tool"].ToString();
        hp = System.Convert.ToSingle(huntable["hp"]);
    }

    public void Kill()
    {
        Item killedItemData = dm.GetItem(killed);
        GameObject killedObj = GameObject.Find("GameManager").GetComponent<ItemSpawner>().SpawnObj(killedItemData);

        killedObj.transform.position = gameObject.transform.position;

        killedObj.name = killedItemData.ID;

        Destroy(gameObject);
    }
}
