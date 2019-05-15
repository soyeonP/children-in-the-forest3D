using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    int playerControll = 3;

    public  bool MoveA;
    public bool MoveB;
    public bool MoveC;
    public bool MoveAll;

    MoveCharacter CharA;
    MoveCharacter CharB;
    MoveCharacter CharC;
    MoveCharacter CharAll;

    MovePointForCam cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<MovePointForCam>();

        // playerMove player_Together = GameObject.Find("Player_Together").GetComponent<playerMove>();
        // playerJoin join_Together = GameObject.Find("Player_Together").GetComponent<playerJoin>(); //전체모임이 선택되었을때, 플레이어를 예쁘게 모이게 해주는아이
       // CharA= GameObject.Find("ACharacter").GetComponent<MoveCharacter>();
       // CharB = GameObject.Find("BCharacter").GetComponent<MoveCharacter>();
       // CharC = GameObject.Find("CCharacter").GetComponent<MoveCharacter>();
        // MoveAll = GameObject.Find("ACharacter").GetComponent<MoveCharacter>().canMove;



    }

    // Update is called once per frame
    void Update()
    {
      
           if (Input.GetKey("q"))
                ChoosePlayer(0);
            if (Input.GetKey("w"))
                ChoosePlayer(1);
            if (Input.GetKey("e"))
                ChoosePlayer(2);
            if (Input.GetKey("r"))
                ChoosePlayer(3);
     
       
        
    }

   void ChoosePlayer(int playerControll)
    {/*
        switch (playerControll)
        {
            case 0:    //playerA
                cam.target = cam.targetA;
               CharA.canMove  = true;
               CharB.canMove  = false;
               CharC.canMove  = false;
               CharAll.canMove= false;
                break;
            case 1:    //playerB
                cam.target = cam.targetB;
               CharA.canMove  = false;
               CharB.canMove  = true;
               CharC.canMove  = false;
                CharAll.canMove = false;
                break;
            case 2:    //playerC 
                cam.target = cam.targetC;
                CharA.canMove = false;
                CharB.canMove  = false;
                CharC.canMove  = true;
                CharAll.canMove = false;
                break;
            case 3:   //전체컨트롤
                      //   Input.GetMouseDown(0); // 마우스 클릭위치로 이동 //일단 집합하고 못움직여야지. 
                cam.target = cam.targetAll;
               CharA.canMove  = false;
               CharB.canMove  = false;
               CharC.canMove  = false;
               CharAll.canMove = true;
                break;
        }
        */
    }
}
