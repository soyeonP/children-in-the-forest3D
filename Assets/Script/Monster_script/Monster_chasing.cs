using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_chasing : MonoBehaviour
{
    public Transform[] waypoints;//몬스터의 경로 지점들
    public float moveSpeed = 2f;//몬스터가 웨이포인트 있는곳들 따라돌때 속도
    int waypointIndex = 0;

    //public Animator empty_stomach;//공복상태일떄의 애니메이션
    //int hunger = 100;//몬스터의 허기상태, 0이 되묜 공복상태로 변한다.

    

    // Start is called before the first frame update

    void Start()
    {
        // empty_stomach = GetComponent<Animator>();
        // Invoke("monster_is_hungry", 10f);
    }


    void Update()
    {
        if (!(gameObject.GetComponentInChildren<vision_of_monster>().thereismeat))
            Move();
        

        /*
        if (hunger == 0)
           empty_stomach.SetBool("starvation", true);

        */

    }

    private void Move()
    {
        if (waypointIndex < waypoints.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position
            , waypoints[waypointIndex].transform.position, moveSpeed * Time.deltaTime);


            if (((int)transform.position.x == (int)waypoints[waypointIndex].transform.position.x)
                && ((int)transform.position.z == (int)waypoints[waypointIndex].transform.position.z))
            { waypointIndex += 1; }
        }
        else
        {
            waypointIndex = 0;
        }
    }

    

   /* void monster_is_hungry()//10초마다 몬스터 허기 1씩 깍기게 start에서 invoke함수로 실행
    {
        hunger -= 100;
    }*/

}