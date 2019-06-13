using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_chasing : MonoBehaviour
{
    public Transform[] waypoints;//몬스터의 경로 지점들
    Vector3 Waypoint_target;//경로 지점 향하는 벡터
    int waypointIndex = 0;

    //public Animator empty_stomach;//공복상태일떄의 애니메이션
    //int hunger = 100;//몬스터의 허기상태, 0이 되묜 공복상태로 변한다.

    Animator anime;

    // Start is called before the first frame update

    void Start()
    {
        anime = GetComponent<Animator>();
        // empty_stomach = GetComponent<Animator>();
        // Invoke("monster_is_hungry", 10f);
    }


    void Update()
    {
        if ((!(gameObject.GetComponentInChildren<vision_of_monster>().thereismeat))&&(gameObject.GetComponentInChildren<vision_of_monster>().count_player_entered == 0))
            Move();

        if ((gameObject.GetComponentInChildren<vision_of_monster>().thereismeat)||(gameObject.GetComponentInChildren<vision_of_monster>().count_player_entered !=0))
            anime.SetBool("mon_walk", false); //독고기가있거나 플레이어가 시야에 들어오면 walk에니매이션 중지

        

        /*
        if (hunger == 0)
           empty_stomach.SetBool("starvation", true);

        */

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

    private void targetting(Transform target)
    {
        Waypoint_target = new Vector3(target.position.x, this.transform.position.y, target.position.z);
    }



    /* void monster_is_hungry()//10초마다 몬스터 허기 1씩 깍기게 start에서 invoke함수로 실행
     {
         hunger -= 100;
     }*/

}