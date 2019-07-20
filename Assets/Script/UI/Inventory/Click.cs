using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Click : MonoBehaviour {

    public GameObject GetBtn;
    public GameObject huntBtn;
    public GameObject checkBtn;
    public GameObject clickedObj;

    public GameObject bullet;
    public Image HuntingUI;

    private MovePointForCam move;

    private AudioSource audio;
    private void Start()
    {
        GetBtn.SetActive(false);
        huntBtn.SetActive(false);
        checkBtn.SetActive(false);

        move = GameObject.Find("Main Camera").GetComponent<MovePointForCam>();

        audio = GetComponent<AudioSource>();
    }

    private void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.collider.gameObject.tag == "Object") // 채집 오브젝트
                {
                    clickedObj = hit.collider.gameObject;
                    GetBtn.SetActive(true); // 클릭 시 채집 버튼 뜨도록

                    Vector3 objPos = Camera.main.WorldToScreenPoint(clickedObj.transform.position);

                    GetBtn.transform.position = new Vector2(objPos.x + 40, objPos.y + 40);
                    //GetBtn.transform.position = new Vector2(clickedObj.transform.position.x + 1, clickedObj.transform.position.y + 1);
                }
                else if (hit.collider.gameObject.tag == "Huntable") // 사냥 오브젝트
                {
                    // 새총 보유중인지 확인
                    int nowChar = move.getMoveChar();
                    if (ObjManager.objManager.inventory.getItemCount("sling", nowChar - 1) > 0)
                    {
                        clickedObj = hit.collider.gameObject;

                        Vector3 objPos = Camera.main.WorldToScreenPoint(clickedObj.transform.position);

                        // 상호작용 버튼 출력
                        huntBtn.SetActive(true);
                        huntBtn.transform.position = new Vector2(objPos.x + 40, objPos.y + 40);
                    }
                }
                else if (hit.collider.gameObject.tag == "Checkable") // 조사 오브젝트
                {

                    clickedObj = hit.collider.gameObject;
                    checkBtn.SetActive(true);

                    Vector3 objPos = Camera.main.WorldToScreenPoint(clickedObj.transform.position);
                    checkBtn.transform.position = new Vector2(objPos.x + 40, objPos.y + 40);
                }
                else
                {
                    GetBtn.SetActive(false);
                    huntBtn.SetActive(false);
                    checkBtn.SetActive(false);
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
            if (checkBtn.activeInHierarchy)
            {
                Vector3 objPos = Camera.main.WorldToScreenPoint(clickedObj.transform.position);
                checkBtn.transform.position = new Vector2(objPos.x + 40, objPos.y + 40);
            }

        }

    }

    
    public void ClickedGetBtn()
    {
        audio.Play();

        // 캐릭터에 따라 조사시간 걸리게 설정 (해야함)
        int charnum = move.getMoveChar();

        if (charnum == -1 || charnum == 0 || charnum == 4) charnum = 1;

        clickedObj.GetComponent<Item>().AddItem(charnum - 1);
        GetBtn.SetActive(false);
        clickedObj = null;
    }

    public void ClickedHuntBtn()
    {
        int character = move.getMoveChar();

        // 사냥 중 UI 나타나도록 함
        HuntingUI.gameObject.SetActive(true);
        IEnumerator fillHuntingUI = FillImage(HuntingUI, character);

        StartCoroutine(fillHuntingUI);
    }

    public void ClickedCheckBtn()
    {
        // 메모 획득
        checkBtn.SetActive(false);

        int charnum = move.getMoveChar();
        if (charnum == -1 || charnum == 0 || charnum == 4) charnum = 1;

        clickedObj.GetComponent<Item>().AddItem(charnum - 1);
    }

    IEnumerator FillImage(Image image, int selChar)
    {
        if (selChar == -1 || selChar == 0 || selChar == 4) selChar = 1;
        GameObject child = DataManager.dataManager.GetChildren()[selChar - 1];
        image.fillAmount = 0;

        while (image.fillAmount < 1)
        {
            Debug.Log("a");
            image.gameObject.transform.position = Camera.main.WorldToScreenPoint(child.transform.position) + new Vector3(0, 40, 0);
            image.fillAmount += 0.05f;
            yield return null;
        }

        image.fillAmount = 0;
        image.gameObject.SetActive(false);

        // 사냥 오브젝트 죽게 함
        clickedObj.GetComponent<Huntable>().Kill();

        huntBtn.SetActive(false);
    }


}
