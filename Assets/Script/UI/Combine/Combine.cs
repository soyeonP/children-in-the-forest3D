using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combine : MonoBehaviour
{
    private bool[] isMaking = { false, false, false };
    private float[] leftTime = { 0, 0, 0 };
    public playerState[] states;

    public GameObject canvas;
    public GameObject makingUI;

    public bool IsMaking(int num) { return isMaking[num - 1]; }
    public float GetLeftTime(int num) { return leftTime[num - 1]; }

    public void StartCombine(string recipeID, int childNum) // 조합 시작
    {
        // 레시피 가져오기
        Dictionary<string,object> recipe = DataManager.dataManager.GetRecipe(recipeID);

        // 재료 아이템 제거
        int matCount = System.Convert.ToInt32(recipe["matCount"]);

        for (int i = 1; i <= matCount; i++)
        {
            for (int k = 0; k < System.Convert.ToInt32(recipe["mat" + i + "Need"]); k++)
            {
                ObjManager.objManager.inventory.RemoveItem(childNum, System.Convert.ToString(recipe["mat" + i + "ID"]));
            }
        }

        /* 아이템 제작 시작 */
        // 진행상황 표시 UI 출력
        GameObject ui = Instantiate(makingUI, canvas.transform);
        ui.transform.position = Camera.main.WorldToScreenPoint(states[childNum - 1].gameObject.transform.position)
            + new Vector3 (0, 20, 0);
        // 위치 조정 필요

        // 캐릭터 아이템 제작 상태
        states[childNum - 1].StartCombine(recipe, ui);
    }

    public void CancleCombine() // 조합 취소
    {
        // 캐릭터 조합 상태 변경
    }

    public void GetCombine() // 조합 완료 후 아이템 획득
    {
        // 캐릭터 조합 상태 변경
        // 인벤토리에 아이템 추가
    }
}
