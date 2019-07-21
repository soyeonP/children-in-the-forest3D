using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionChecker : MonoBehaviour
{
    // 현재 시야 내 플레이어 수
    public int cntEnteredChild;        
    public int CntEnteredChild
    {
        get;
    }

    private GameObject chasingChild;                    // 추적 중인 플레이어
    public List<GameObject> meat;                       // 시야 내의 독고기 리스트

    private Werewolf wolfController;
    private SphereCollider expandedVisionCollider;      // 냄새 추적 스킬 발동 시 시야 범위
    private BoxCollider normalVisionCollider;


    public GameObject GetChasingChild() { return chasingChild; }
    public void SetVisionExpanded(bool isExpanded)
    {
        expandedVisionCollider.enabled = isExpanded;

        cntEnteredChild = 0;
        ChangeChasingChild(null);

        normalVisionCollider.enabled = !isExpanded;
    }


    private void Start()
    {
        cntEnteredChild = 0;

        wolfController = GetComponentInParent<Werewolf>();

        expandedVisionCollider = GetComponent<SphereCollider>();
        expandedVisionCollider.enabled = false;

        normalVisionCollider = GetComponent<BoxCollider>();
        normalVisionCollider.enabled = true;

        meat = new List<GameObject>();
    }

    private void ChangeChasingChild(GameObject chasingChild)
    {
        this.chasingChild = chasingChild;
        wolfController.SetChasingChild(chasingChild);
    }

    private void OnTriggerEnter(Collider col)
    {
        switch (col.gameObject.tag)
        {
            // 시야 내 플레이어 들어왔을 경우
            case "Player":

                if (cntEnteredChild++ == 0)
                {
                    ChangeChasingChild(col.gameObject);
                    wolfController.SetChasingMode(true);
                }

                Debug.Log("in : " + cntEnteredChild);

                break;

            // 시야 내 독고기 들어왔을 경우
            case "trap":

                if (col.gameObject.GetComponent<ItemController>().item.ID == "poison_meat" && !meat.Contains(col.gameObject))
                    meat.Add(col.gameObject);

                break;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        switch (col.gameObject.tag)
        {
            case "Player":
                cntEnteredChild--;

                if (Equals(col.gameObject, chasingChild))
                {
                    ChangeChasingChild(null);
                }

                Debug.Log("out : " + cntEnteredChild);

                break;

            case "trap":
                meat.Remove(col.gameObject);
                break;
        }
    }
}
