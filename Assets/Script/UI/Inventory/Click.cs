using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Click : MonoBehaviour {

    public GameObject GetBtn;
    public GameObject clickedObj;

    private void Start()
    {
        GetBtn.SetActive(false);
    }

    private void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.collider.gameObject.tag == "Object") // 오브젝트 클릭했으면
                {
                    clickedObj = hit.collider.gameObject;
                    GetBtn.SetActive(true); // 클릭 시 채집 버튼 뜨도록
                    GetBtn.transform.position = new Vector2(mousePosition.x + 20, mousePosition.y + 20);
                    //GetBtn.transform.position = new Vector2(clickedObj.transform.position.x + 1, clickedObj.transform.position.y + 1);
                }
                else
                {
                    GetBtn.SetActive(false);
                    clickedObj = null;
                }
            }
        }
	}

    public void ClickedGetBtn()
    {
        // 캐릭터에 따라조사시간 걸리게 설정 (해야함)
        Debug.Log("a");
        int charnum = GameObject.Find("Main Camera").GetComponent<MovePointForCam>().getMoveChar();

        if (charnum == -1 || charnum == 0 || charnum == 4) charnum = 1;

        clickedObj.GetComponent<Item>().AddItem(charnum - 1);
        GetBtn.SetActive(false);
        clickedObj = null;
    }
}
