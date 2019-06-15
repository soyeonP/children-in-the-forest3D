﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Click : MonoBehaviour {

    public GameObject GetBtn;
    public GameObject huntBtn;
    public GameObject clickedObj;

    public GameObject bullet;

    private MovePointForCam move;

    private void Start()
    {
        GetBtn.SetActive(false);
        move = GameObject.Find("Main Camera").GetComponent<MovePointForCam>();
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

                    Vector3 objPos = Camera.main.WorldToScreenPoint(clickedObj.transform.position);

                    GetBtn.transform.position = new Vector2(objPos.x + 40, objPos.y + 40);
                    //GetBtn.transform.position = new Vector2(clickedObj.transform.position.x + 1, clickedObj.transform.position.y + 1);
                }
                else if (hit.collider.gameObject.tag == "Huntable")
                {
                    // TODO 새총 보유중인지 확인
                    int nowChar = move.getMoveChar();
                    //if (ObjManager.objManager.inventory.getItemCount("slinger", nowChar) > 0) {
                    // 아직 새총 추가 안돼서 조건 안 걸어둠
                        clickedObj = hit.collider.gameObject;

                    Vector3 objPos = Camera.main.WorldToScreenPoint(clickedObj.transform.position);

                    // 상호작용 버튼 출력
                    huntBtn.SetActive(true);
                    huntBtn.transform.position = new Vector2(objPos.x + 40, objPos.y + 40);
                    //}
                }
                else
                {
                    GetBtn.SetActive(false);
                    huntBtn.SetActive(false);
                    clickedObj = null;
                }
            }
        }

        if (clickedObj != null)
        {
            if (GetBtn.activeInHierarchy)
            {
                Vector3 objPos = Camera.main.WorldToScreenPoint(clickedObj.transform.position);
                GetBtn.transform.position = new Vector2(objPos.x + 40, objPos.y + 40);
            }
            if (huntBtn.activeInHierarchy)
            {

                Vector3 objPos = Camera.main.WorldToScreenPoint(clickedObj.transform.position);
                huntBtn.transform.position = new Vector2(objPos.x + 40, objPos.y + 40);
            }
        }
	}

    public void ClickedGetBtn()
    {
        // 캐릭터에 따라 조사시간 걸리게 설정 (해야함)
        Debug.Log("a");
        int charnum = GameObject.Find("Main Camera").GetComponent<MovePointForCam>().getMoveChar();

        if (charnum == -1 || charnum == 0 || charnum == 4) charnum = 1;

        clickedObj.GetComponent<Item>().AddItem(charnum - 1);
        GetBtn.SetActive(false);
        clickedObj = null;
    }

    public void ClickedHuntBtn()
    {
        // 탄환 발사 이펙트
        //GameObject bul = Instantiate(bullet);
        //bul.GetComponent<Rigidbody>().AddForce


        // 일단 걍 토끼가 죽게 하자
        clickedObj.GetComponent<Huntable>().Kill();
        // 사냥 버튼 끄기
        huntBtn.SetActive(false);
    }
}
