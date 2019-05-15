using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePointForCam : MonoBehaviour
{
    public Transform ClickPoint;

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
   // MoveCharacter CharAll;
    JointoTogether CharAll_Join;

    public int MoveChar = 0 ; // 1 : A , 2 : B , 3: C 0: All  
    
    //camera limit 
    public const float offsetX = 0f;
    public const float offsetY = 20f;
    public const float offsetZ = 0f;
    public float limitright = 10f;
    //public float limitleft = -10f;
    public float limittop = 10f;
    //public float limitbottom = -10f;

    public float followSpeed = 10.0f;

    private Transform tr;
    Vector3 cameraPosition;

    GameObject top;
    GameObject left;
    GameObject right;

    public float[] dirLen;

    private void Start()
    {
        tr = GetComponent<Transform>();
        target = targetA;

        CharA= GameObject.Find("ACharacter").GetComponent<MoveCharacter>();
        CharB = GameObject.Find("BCharacter").GetComponent<MoveCharacter>();
        CharC = GameObject.Find("CCharacter").GetComponent<MoveCharacter>();
       // CharAll = GameObject.Find("Together").GetComponent<MoveCharacter>();
        CharAll_Join = GameObject.Find("Together").GetComponent<JointoTogether>();
    }

    void Update()
    {
        //follow Character 
        cameraPosition = new Vector3(target.position.x + 2f , offsetY, target.position.z -9f );

        if (target.position.x +2f > limitright)
            cameraPosition.x  = limitright;
        else if (target.position.x +2f < -(limitright))
            cameraPosition.x = -(limitright);

        if (target.position.z -9f> limittop)
            cameraPosition.z = limittop;
        else if (target.position.z -9f < -(limittop))
            cameraPosition.z = -(limittop);

        transform.position = Vector3.Lerp(tr.position, cameraPosition, followSpeed * Time.deltaTime);

        /*character choose */ 
        if (Input.GetKey("q"))
        {
            target = targetA;
            CharA.canMove = true;
            CharB.canMove = false;
            CharC.canMove = false;
            CharAll_Join.join = false;
        }
        if (Input.GetKey("w"))
        {
            target = targetB;
            CharA.canMove = false;
            CharB.canMove = true;
            CharC.canMove = false;
            CharAll_Join.join = false;
        }
        if (Input.GetKey("e"))
        {
            target = targetC;
            CharA.canMove = false;
            CharB.canMove = false;
            CharC.canMove = true;
            CharAll_Join.join = false;
        }
        
        if (Input.GetKey("r"))
        {
           // target = targetAll;
            CharA.canMove = false;
            CharB.canMove = false;
            CharC.canMove = false;
          // CharAll.canMove = true;
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
      /*  if (CharAll.canMove == true)
        {
            MoveChar = 0;
            getPosition();        
        }*/
        if(CharAll_Join.join == true)
        {
            MoveChar = 4;
            getPosition();
        }
    }

    void getPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit Hit;

            if (Physics.Raycast(ray, out Hit, Mathf.Infinity))
            {
                Debug.Log(Hit.point);

                switch (MoveChar)
                {
                    case 0:    //playerAll
                               // target = targetAll;
                       // CharAll.position = Hit.point;
                        break;
                    case 1:    //playerA
                        target = targetA;
                        CharA.position = Hit.point;
                        break;
                    case 2:    //playerB
                        target = targetB;
                        CharB.position = Hit.point;
                        break;
                    case 3:   //playerC
                              //   Input.GetMouseDown(0); // 마우스 클릭위치로 이동 //일단 집합하고 못움직여야지. 
                        target = targetC;
                        CharC.position = Hit.point;
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

  void  setPosition( Vector3 joinPos) // join해야할때, 애들 위치 설정 
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
        // 캐릭터 이동값은 transform.position의 위아래옆 으로 모인다.

        //       CharA.position = top.transform.position;
        //       CharB.position = left.transform.position;
        //       CharC.position = right.transform.position;

        //  join = false;
        // 위 아래 옆 위치로 도착하면
        //그 위치에 together의 자식화 
        //그리고 together의 movecharacter의 canmove를 킨다. 


    }


    public float DistanceToPoint(Vector3 a, Vector3 b)
    {
        Debug.Log("연산문");
        return (float)Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.z - b.z, 2));
    }

}
