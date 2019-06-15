using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayer : MonoBehaviour
{
    private MovePointForCam move;

    private void Start()
    {
        move = GameObject.Find("Main Camera").GetComponent<MovePointForCam>();
    }

    public void onClickPlayerBtn(int num)
    {
        move.MoveChar = num;
    }
}
