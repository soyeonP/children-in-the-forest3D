using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerState : MonoBehaviour
{
    public float hp = 100;              // 체력
    public float full = 100;             // 포만감
    public float scare = 10;            // 공포
    public int insight;                 // 통찰력
    public int charNum;

    public Slider hpBar;
    public Slider fullBar;
    public Slider scareBar;

    public GameObject dead_char;
    public GameObject dead_state;

    private bool isSelected = false;    // 현재 선택되었는가 여부

    /* 조합 부분 */
    public bool isWorking = false;     // 작업중 여부

    private int min, sec;      // 남은 조합 시간
    private float dt;
    private Dictionary<string, object> recipe; // 만들고 있는 아이템 레시피 정보
    private Text txtLeftTime;
    private GameObject ui;
    public GameObject askMsg;

    private SaveLoad saveNLoad;

    public bool IsSelected() { return isSelected; }
    public bool IsWorking() { return isWorking; }

    public void SetWorking() { isWorking = !isWorking; }

    public void StartCombine(Dictionary<string, object> recipe, GameObject ui)
    {
        SetWorking();

        int time = System.Convert.ToInt32(recipe["time"]);
        min = time / 60;
        sec = time % 60;

        this.recipe = recipe;
        this.ui = ui;
        ui.transform.GetChild(0).GetComponent<Image>().sprite = DataManager.dataManager.findSprite(System.Convert.ToString(recipe["comID"]));
        txtLeftTime = ui.transform.GetChild(1).GetComponent<Text>();
        txtLeftTime.text = min + " : " + sec;

        txtLeftTime.transform.parent.GetComponent<Button>().onClick.AddListener(() =>
        { 
            askMsg.SetActive(true);
            askMsg.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
            {
                CancleCombine();
                askMsg.SetActive(false);
            });
        });
    }

    private void CompleteCombine()
    {
        txtLeftTime.text = "제작완료!";

        txtLeftTime.transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
        txtLeftTime.transform.parent.GetComponent<Button>().onClick.AddListener(GetCombItem);
    }

    private void CancleCombine()
    {
        SetWorking();
        Destroy(ui);
        ui = null;
    }

    private void GetCombItem()
    {

        // 인벤토리 가득찼는지 여부 확인
        if (ObjManager.objManager.inventory.AddItem(charNum - 1, DataManager.dataManager.GetItem(System.Convert.ToString(recipe["comID"]))))
        {
            SetWorking();
            Destroy(ui);
            ui = null;
            Debug.Log("획득 완료");
        }
        else
        {
            txtLeftTime.text = "자리 부족";
            Debug.Log("자리 없음");
        }
    }

    /* 조합 부분 끝 */
 

    void Update()
    {

        if (full > 0) 
            full -= (0.01f *Time.deltaTime);
        else
            dead();
        //몬스터가 공격시 hp 감소
        if (hp > 0)
            ;
        else
            dead();

        if (scare > 0)
            ;
        else
            dead();

        //플레이어 사망 클래스만들고/ 클래스 소환. 
        //음식아이템 섭취시 stamina 증가

        if (ui != null) ui.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        // 조합 중일 시 시간 ui 설정
        if (isWorking)
        {
            dt += Time.deltaTime;

            if (dt >= 1)
            {
                dt = 0;
                if (--sec < 0)
                {
                    if (--min <= 0)
                    {
                        CompleteCombine(); // 조합 완료 함수
                        return;
                    }

                    sec = 59;
                }

                txtLeftTime.text = min + " : " + sec;
            }
        }

        hpBar.value = hp;
        fullBar.value = full;
        scareBar.value = scare;
    }

    public void ChangeHP(float value)
    {
        // hp 확인 구간, value 값은 음수도 올 수 있는 것 유의할 것!!!!!!
        /*
         * if ((hp += value) > maxHP) hp = maxHP;
         * else if (hp <= 0) 체력 0 이하일 시 함수 호출
         */

        hp += value;
    }

    public void ChangeFull(float value)
    {
        // full 확인 구간, full 값은 음수도 가능!!!!!!!!!
        /*
         * if ((full += value) > maxFull) full = maxFull;
         * else if (full <= 0) 포만감 0 이하일 시 함수 호출
         */

        full += value;
    }

    public float GetHP()
    {
     
        return hp;
    }
    public float GetFull()
    {
     
        return full;
    }
    public float GetScare()
    {

        return scare;
    }

    private void dead()
    {
        hp = 0;
        full = 0;
        scare = 0;
        gameObject.SetActive(false);
        setAct();
    }

    public void setAct()
    {
        dead_char.SetActive(true);
        dead_state.SetActive(true);
    }
}
