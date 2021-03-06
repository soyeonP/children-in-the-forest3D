﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovePointForCam : MonoBehaviour
{
    public GameObject Panels;

    /* camera target */
    public Transform target;
    public Transform targetA;
    public Transform targetB;
    public Transform targetC;
    public Transform targetAll;

    /*each character */
    MoveCharacter CharA;
    MoveCharacter CharB;
    MoveCharacter CharC;
    JointoTogether CharAll_Join;

    public int MoveChar = 0; // 1 : A , 2 : B , 3: C 0: All  

    //camera limit 
    public const float offsetX = 0f;
    public const float offsetY = 15f;
    public const float offsetZ = 0f;
    public float limitright = 100f;
    //public float limitleft = -10f;
    public float limittop = 100f;
    //public float limitbottom = -10f;

    public float followSpeed = 10.0f;

    private Transform tr;
    Vector3 cameraPosition;
    Vector3 movePoint;

    GameObject top;
    GameObject left;
    GameObject right;

    public float[] dirLen;

    public int getMoveChar()
    {
        return MoveChar;
    }

    private void Start()
    {

        tr = GetComponent<Transform>();
        target = targetA;

        transform.localEulerAngles = new Vector3(65, 0, 0);


          CharA = GameObject.Find("ACharacter").GetComponent<MoveCharacter>();
        CharB = GameObject.Find("BCharacter").GetComponent<MoveCharacter>();
        CharC = GameObject.Find("CCharacter").GetComponent<MoveCharacter>();
        CharAll_Join = GameObject.Find("Together").GetComponent<JointoTogether>();
    }

    void Update()
    {
        //follow Character 
        cameraPosition = new Vector3(target.position.x , offsetY, target.position.z - 7 );

        if (target.position.x + 2f > limitright)
            cameraPosition.x = limitright;
        else if (target.position.x + 2f < -(limitright))
            cameraPosition.x = -(limitright);

        if (target.position.z - 9f > limittop)
            cameraPosition.z = limittop;
        else if (target.position.z - 9f < -(limittop))
            cameraPosition.z = -(limittop);

        transform.position = Vector3.Lerp(tr.position, cameraPosition, followSpeed * Time.deltaTime);

        /*character choose */

        if (Input.GetKey("q") || MoveChar == 1)
        {
            target = targetA;
            CharA.canMove = true;
            CharB.canMove = false;
            CharC.canMove = false;
            CharAll_Join.join = false;
        }
        if (Input.GetKey("w") || MoveChar == 2)
        {
            target = targetB;
            CharA.canMove = false;
            CharB.canMove = true;
            CharC.canMove = false;
            CharAll_Join.join = false;
        }
        if (Input.GetKey("e") || MoveChar == 3)
        {
            target = targetC;
            CharA.canMove = false;
            CharB.canMove = false;
            CharC.canMove = true;
            CharAll_Join.join = false;
        }
        
        if (Input.GetKey("r") || MoveChar == 4)
        {
           // target = targetAll;
            CharA.canMove = false;
            CharB.canMove = false;
            CharC.canMove = false;
           CharAll_Join.join = true;
        }



        //Get Point to move for character      
        if (CharA.canMove == true )
        {
            MoveChar = 1;
            getPosition();
        }
        if (CharB.canMove == true)
        {
            MoveChar = 2;
            getPosition();

        }
        if (CharC.canMove == true)
        {
            MoveChar = 3;
            getPosition();
            
        }
        if(CharAll_Join.join == true)
        {
            MoveChar = 4;
            getPosition();
        }
    }

    void getPosition()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.currentSelectedGameObject 
            && !Panels.transform.GetChild(0).gameObject.activeInHierarchy
            && !Panels.transform.GetChild(1).gameObject.activeInHierarchy) // 인벤토리, 제작 UI 안 켜져 있을 때만 이동하도록!!
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit Hit;

            if (Physics.Raycast(ray, out Hit, Mathf.Infinity))
            {
                Debug.Log(Hit.point);

                if (Hit.collider.name == "Plane" || Hit.collider.tag == "attack" || Hit.collider.tag =="Finish"
                    && Hit.collider.tag != "Object" && Hit.collider.tag != "Checkable" && Hit.collider.tag != "Huntable")
                {
                    movePoint = new Vector3(Hit.point.x, 0, Hit.point.z);
                    Debug.Log(movePoint);
                    switch (MoveChar)
                    {
                        case 0:

                            break;
                        case 1:    //playerA
                            target = targetA;
                            CharA.position = movePoint;
                            break;
                        case 2:    //playerB
                            target = targetB;
                            CharB.position = movePoint;
                            break;
                        case 3:   //playerC
                                  //   Input.GetMouseDown(0); // 마우스 클릭위치로 이동 //일단 집합하고 못움직여야지. 
                            target = targetC;
                            CharC.position = movePoint;
                            break;
                        case 4:
                            // target = targetAll;
                            CharAll_Join.transform.position = Hit.point;
                            setPosition(CharAll_Join.transform.position);

                            break;
                    }
                }
            }
        }
    }


    /* join해야할때, 애들 위치 설정 */
    void setPosition( Vector3 joinPos)
    {
        
        top = GameObject.FindGameObjectWithTag("top");
        left = GameObject.FindGameObjectWithTag("left");
        right = GameObject.FindGameObjectWithTag("right");

        Vector3 topPos = top.transform.position;
        Vector3 leftPos = left.transform.position;
        Vector3 rightPos = right.transform.position;

        float t1l2r3 = DistanceToPoint(topPos, CharA.transform.position) + DistanceToPoint(leftPos, CharB.transform.position) + DistanceToPoint(rightPos, CharC.transform.position);
        float t1r2l3 = DistanceToPoint(topPos, CharA.transform.position) + DistanceToPoint(leftPos, CharC.transform.position) + DistanceToPoint(rightPos, CharB.transform.position);
        float t2l1r3 = DistanceToPoint(topPos, CharB.transform.position) + DistanceToPoint(leftPos, CharA.transform.position) + DistanceToPoint(rightPos, CharC.transform.position);
        float t2l3r1 = DistanceToPoint(topPos, CharB.transform.position) + DistanceToPoint(leftPos, CharC.transform.position) + DistanceToPoint(rightPos, CharA.transform.position);
        float t3l1r2 = DistanceToPoint(topPos, CharC.transform.position) + DistanceToPoint(leftPos, CharA.transform.position) + DistanceToPoint(rightPos, CharB.transform.position);
        float t3l2r1 = DistanceToPoint(topPos, CharC.transform.position) + DistanceToPoint(leftPos, CharB.transform.position) + DistanceToPoint(rightPos, CharA.transform.position);

        dirLen = new float[6];
        dirLen[0] = t1l2r3;
        dirLen[1] = t1r2l3;
        dirLen[2] = t2l1r3;
        dirLen[3] = t2l3r1;
        dirLen[4] = t3l1r2;
        dirLen[5] = t3l2r1;

        Debug.Log(dirLen[0]);

        int min = 0;
        for (int i = 1; i < 6; i++)
        {
            if (dirLen[min] > dirLen[i])
                min = i;
            Debug.Log("작은거 " + dirLen[min]);
        }

        switch (min)
        {

            case 0:
                CharA.position = topPos;
                CharB.position = leftPos;
                CharC.position = rightPos;

                break;
            case 1:
                CharA.position = topPos;
                CharB.position = rightPos;
                CharC.position = leftPos;

                break;
            case 2:
                CharA.position = leftPos;
                CharB.position = topPos;
                CharC.position = rightPos;

                break;
            case 3:
                CharA.position = rightPos;
                CharB.position = topPos;
                CharC.position = leftPos;

                break;
            case 4:
                CharA.position = leftPos;
                CharB.position = rightPos;
                CharC.position = topPos;

                break;
            case 5:
                CharA.position = rightPos;
                CharB.position = leftPos;
                CharC.position = topPos;

                break;
        }

    }

    /* 가까운 거리 연산 */
    public float DistanceToPoint(Vector3 a, Vector3 b)
    {      
        return (float)Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.z - b.z, 2));
    }

}
