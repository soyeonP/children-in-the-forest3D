using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineUI : MonoBehaviour
{
    private DataManager dm;
    private MovePointForCam move;
    private Combine cb;

    private List<Dictionary<string, object>> recipeList;
    private Dictionary<string, object> recipe;

    public GameObject recipePrefab;
    public GameObject recipeParent;

    public GameObject combinePanel;
    public GameObject itemInfo;
    public GameObject costInfo;
    public Button btnCombine;
    private bool combinable;

    public List<playerState> playerStates; // 임시로 플레이어 상태 가져오기 위해 리스트 선언!!!

    private int selectedChar; // 현재 선택된 캐릭터 넘버, 미선택 시 -1
    public GameObject BtnChars;
    public Color Shadow;

    private void Start()
    {
        /* 임시 설정 */

        dm = DataManager.dataManager;
        cb = GetComponent<Combine>();
        recipeList = dm.GetRecipeList();

        move = GameObject.Find("Main Camera").GetComponent<MovePointForCam>();

        InitCombiner();

        // 레시피 리스트 개수만큼 버튼 생성
        for (int i = 0; i < recipeList.Count; i++)
        {
            Dictionary<string, object> recipe = recipeList[i];
            MakeBtnRecipe(recipe);
        }

        // 다른 일 하고 있는 캐릭터 있는지 체크하고 있다면 다른일 리본 띄우기
    }

    public void SetRecipeBtn()
    {
        for (int i = 0; i < recipeParent.transform.childCount; i++)
        {
            GameObject newRecipe = recipeParent.transform.GetChild(i).gameObject;

            if (PlayerPrefs.GetInt("isGot" + newRecipe.name, 0) == 0)
            { // 레시피 획득하지 않은 경우 버튼 선택 비활성화
                newRecipe.GetComponent<Button>().interactable = false;
            }
            else
            {
                newRecipe.GetComponent<Button>().interactable = true;
            }

        }
    }

    public void InitCombiner()
    {
        // 변수 초기화
        recipe = null;
        selectedChar = 0;
        btnCombine.interactable = false;
        combinable = false;

        for (int i = 0; i < 3; i++)
        {
            if (playerStates[i].IsWorking())
            {
                BtnChars.transform.GetChild(i + 1).GetChild(1).gameObject.SetActive(true);
            }
        }

        costInfo.SetActive(false);
        itemInfo.SetActive(false);

        // 레시피 획득 여부 확인하고 isGot에 1 들어간 애들 활성화 시켜줘

        // 현재 선택된 캐릭터 기본 선택되도록 함
        SelectChild(move.getMoveChar());
        if (selectedChar == 0) SelectChild(1);

        // 만약 전체 컨트롤 하고 있을 시 통찰력 제일 높은 아이로 설정
        if (selectedChar == 4)
        {
            int maxIndex = 0;

            for (int i = 0; i < playerStates.Count; i++)
            {
                if (playerStates[maxIndex].insight < playerStates[i].insight)
                {
                    maxIndex = i;
                }
            }

            SelectChild(maxIndex + 1);
        }

        SetCharBtnSelected(selectedChar);

        SetRecipeBtn();
    }

    private void MakeBtnRecipe(Dictionary<string, object> recipe)
    {
        GameObject newRecipe = Instantiate(recipePrefab, recipeParent.transform); // 버튼 인스턴스 생성
        newRecipe.name = System.Convert.ToString(recipe["comID"]);

        // 아이템 이미지 설정
        newRecipe.transform.GetChild(0).GetComponent<Image>().sprite = dm.findSprite(newRecipe.name);
        // 아이템 이름 설정
        newRecipe.transform.GetChild(1).GetComponent<Text>().text = dm.getName(newRecipe.name);
        newRecipe.GetComponent<Button>().onClick.AddListener(() => onClickRecipe(recipe));

        //if (System.Convert.ToInt16(recipe["isGot"]) == 0)
        if (PlayerPrefs.GetInt("isGot" + newRecipe.name, 0) == 0)
        { // 레시피 획득하지 않은 경우 버튼 선택 비활성화
            newRecipe.GetComponent<Button>().interactable = false;
        }
        else
        {
            newRecipe.GetComponent<Button>().interactable = true;
        }
    }

    private void onClickRecipe(Dictionary<string, object> recipe)
    {
        string id = System.Convert.ToString(recipe["comID"]);
        this.recipe = recipe;
        Item item = dm.GetItem(id);


        /* 선택 아이템 정보 창 설정 */

        itemInfo.SetActive(true);
        // 스프라이트 설정
        itemInfo.transform.GetChild(0).GetComponent<Image>().sprite = dm.findSprite(id);
        // 아이템 이름 설정
        itemInfo.transform.GetChild(1).GetComponent<Text>().text = item.name;
        // 아이템 종류 설정
        itemInfo.transform.GetChild(2).GetComponent<Text>().text = item.type.ToString();
        // 아이템 설명 설정
        itemInfo.transform.GetChild(3).GetComponent<Text>().text = item.effect;


        /* 아이템 제작 정보 창 설정 */

        costInfo.SetActive(true);

        // 재료 설정
        int matCount = System.Convert.ToInt16(recipe["matCount"]);

        for (int i = 0; i < matCount; i++)
        {
            costInfo.transform.GetChild(i).gameObject.SetActive(true);

            Item item_mat = dm.GetItem(System.Convert.ToString(recipe["mat" + (i + 1) + "ID"]));
            Transform mat = costInfo.transform.GetChild(i);
            mat.gameObject.name = item_mat.ID;
            mat.GetChild(0).GetComponent<Image>().sprite = dm.findSprite(item_mat.ID);      // 재료 스프라이트
            mat.GetChild(2).GetComponent<Text>().text = " / " +                             // 필요 재료 수
                System.Convert.ToString(recipe["mat" + (i + 1) + "Need"]);
        }
        for (int i = matCount; i < 2; i++)
        {
            costInfo.transform.GetChild(i).gameObject.SetActive(false);
        }

        // 제작 시간 설정
        costInfo.transform.GetChild(3).GetComponent<Text>().text = "제작 시간 : " +
            System.Convert.ToString(recipe["time"]) + "초";

        // 필요 통찰력 설정
        costInfo.transform.GetChild(4).GetChild(1).GetComponent<Text>().text
            = " / " + System.Convert.ToInt16(recipe["insight"]).ToString();

        setCharInfo(); // 현재 캐릭터별 소지 재료 수와 통찰력 설정
    }

    private void setCharInfo()
    {
        combinable = true;

        // 통찰력 설정
        if (selectedChar == 0)     // 선택되지 않았을 경우
        {
            combinable = false;

            Text nowInsight = costInfo.transform.GetChild(4).GetChild(0).GetComponent<Text>();
            nowInsight.text = "?";
            nowInsight.color = Color.red;
        }
        else if (selectedChar == 4) // 전체 제어중일 경우
        {
            int insightIndex = 0;

            for (int i = 0; i < playerStates.Count; i++)
            {
                if (playerStates[insightIndex].insight < playerStates[i].insight) insightIndex = i;
            }

            Text nowInsight = costInfo.transform.GetChild(4).GetChild(0).GetComponent<Text>();

            int insight = playerStates[insightIndex].insight;
            nowInsight.text = insight.ToString();

            if (System.Convert.ToInt16(recipe["insight"]) > insight)
            {
                nowInsight.color = Color.red;
                combinable = false;
            }
            else nowInsight.color = Color.black;
        }
        else                        // 플레이어 선택했을 경우
        {
            Text nowInsight = costInfo.transform.GetChild(4).GetChild(0).GetComponent<Text>();

            int insight = playerStates[selectedChar - 1].insight;
            nowInsight.text = insight.ToString();

            if (System.Convert.ToInt16(recipe["insight"]) > insight)
            {
                nowInsight.color = Color.red;
                combinable = false;
            }
            else nowInsight.color = Color.black;
        }

        // 소지 재료 수 설정
        if (itemInfo.activeInHierarchy)
        {
            for (int i = 0; i < System.Convert.ToInt16(recipe["matCount"]); i++)
            {
                Item item_mat = dm.GetItem(System.Convert.ToString(recipe["mat" + (i + 1) + "ID"]));
                Transform mat = costInfo.transform.GetChild(i);
                Text txtNowMat = mat.GetChild(1).GetComponent<Text>();

                int nowMat;

                if (selectedChar == 0) nowMat = 0;
                else nowMat = ObjManager.objManager.inventory.getItemCount(item_mat.ID, selectedChar - 1);

                txtNowMat.text = nowMat.ToString();

                if (System.Convert.ToInt16(recipe["mat" + (i + 1) + "Need"]) > nowMat)
                {
                    txtNowMat.color = Color.red;
                    combinable = false;
                }
                else
                {
                    txtNowMat.color = Color.black;
                }
            }

            // 제작 가능 여부 결정
            checkCombinable();
        }
    }

    private void checkCombinable()
    {
        if (combinable) btnCombine.interactable = true;
        else btnCombine.interactable = false;
    }

    public void SelectChild(int num)
    {
        selectedChar = num;
        SetCharBtnSelected(selectedChar);

        // 제작 아이템 정보창 업데이트
        if (itemInfo.activeInHierarchy) setCharInfo();
    }

    public void StartCombine()
    {
        Debug.Log("조합 시작!");

        combinePanel.SetActive(false);
        cb.StartCombine(System.Convert.ToString(recipe["comID"]), selectedChar);
    }

    private void SetCharBtnSelected(int n)
    {

        for (int i = 1; i < BtnChars.transform.childCount; i++)
        {
            if (i == n) BtnChars.transform.GetChild(i).GetComponent<Image>().color = Shadow;
            else BtnChars.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }

    }
}
