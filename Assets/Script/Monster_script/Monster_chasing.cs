using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_chasing : MonoBehaviour
{
    public Transform[] waypoints;//몬스터의 경로 지점들
    Vector3 Waypoint_target;//경로 지점 향하는 벡터
    int waypointIndex = 0;

    
    public int hunger = 100;//몬스터의 허기상태, 0이 되묜 공복상태로 변한다.
    public int mon_HP = 100;//몬스터 체력
    public bool mon_empty_stomach = false;
    bool empty_stomach_temp = false; //몬스터 공복상태애니메이션 발동할동안 move함수 잠깐멈추게하려는 임시bool
    bool for_once = true; //update()의 hunger조건의 조건문 한번만 실행하기 위한 임시bool

    Animator anime;

    // Start is called before the first frame update

    void Start()
    {
        anime = GetComponent<Animator>();
        InvokeRepeating("monster_is_hungry", 1f, 5f);//몬스터 hp깎이게하는 함수 반복호출
    }


    void Update()
    {
        
        //thereisemeat(독고기 bool)과 count_play가 0일때(캐릭터가 시야에 없을떄)와 공복상태 아닐때 이3가지 조건만족해야 move함수로 몬스터가 경로돔 
        if ((!(gameObject.GetComponentInChildren<vision_of_monster>().thereismeat))&&(gameObject.GetComponentInChildren<vision_of_monster>().count_player_entered == 0)&&!(empty_stomach_temp))
            Move();

        if ((gameObject.GetComponentInChildren<vision_of_monster>().thereismeat)||(gameObject.GetComponentInChildren<vision_of_monster>().count_player_entered !=0)||(empty_stomach_temp))
            anime.SetBool("mon_walk", false); //독고기가있거나 플레이어가 시야에 들어오거나 공복상태면 walk에니매이션 중지

        if ((hunger == 0) && for_once)
        {
            CancelInvoke("monster_is_hungry");
            mon_empty_stomach = true;
            empty_stomach_temp = true; //위에 move함수랑walk애니메이션 멈추게하려는 임시변수
            anime.Play("Hungry");
            Invoke("delay_move", 3.5f);
            for_once = false;
        }

        if(mon_HP == 0)
        {
            gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
            Destroy(gameObject, 3f);
        }
        

    }

    private void delay_move()
    {
        empty_stomach_temp = false;//다시 move함수 실행하게함
    }

    private void Move()
    {
        anime.SetBool("mon_walk", true);
        if (waypointIndex < waypoints.Length)
        {
            targetting(waypoints[waypointIndex]);
            gameObject.transform.LookAt(Waypoint_target);
            gameObject.transform.Translate(Vector3.forward * 0.1f);
          


            if (((int)transform.position.x == (int)waypoints[waypointIndex].transform.position.x)
                && ((int)transform.position.z == (int)waypoints[waypointIndex].transform.position.z))
            { waypointIndex += 1; }
        }
        else
        {
            waypointIndex = 0;
        }
        
    }

    private void targetting(Transform target) //Lookat으로 쫓아갈 타겟설정하는 함수
    {
        Waypoint_target = new Vector3(target.position.x, this.transform.position.y, target.position.z);
    }



     void monster_is_hungry()//10초마다 몬스터 허기 1씩 깍기게 start에서 invoke함수로 실행
     {
         hunger -= 10;
     }

    

}