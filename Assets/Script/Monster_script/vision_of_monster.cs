﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vision_of_monster : MonoBehaviour
{
    public GameObject monster;
    public GameObject[] Player = new GameObject[3];
    public GameObject player_monster_chased;

    Vector3[] distance = new Vector3[3];//플레이어와 몬스터간의 거리벡터

    float[] sqrdistance = new float[3];//거리벡터를 제곱해서 거리를 수치화시킴
    float Min;//몬스터랑 가장 가까운 플레이어 구하기 위해서

    public GameObject poisened_meat;   //독고기
    public bool thereismeat = false;

    public Vector3 targetPosition; //쫓을 대상(밑에targetsetting함수로 정함)

    Animator anime;


    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    public int count_player_entered = 0;

    private void OnTriggerEnter(Collider coll3)
    {
        if (coll3.tag == "Player")
        {
            count_player_entered++;
        }

        if (coll3.tag == "pmeat")
        {
            thereismeat = true;
        }
    }


    private void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "pmeat")
        {
            targetsetting(poisened_meat);
            GameObject.FindGameObjectWithTag("monster").transform.LookAt(targetPosition);
            GameObject.FindGameObjectWithTag("monster").transform.Translate(Vector3.forward * 0.08f);
            anime.SetBool("mon_run", true);
        }


        else if (coll.tag == "Player")
        {
            if(count_player_entered == 1)
            {
                if (!thereismeat)
                {
                    targetsetting(coll.gameObject);
                    monster.transform.LookAt(targetPosition);
                    monster.transform.Translate(Vector3.forward * 0.08f);
                    anime.SetBool("mon_walk", true);
                }
            }

            else
            {
                if (!thereismeat)
                {
                    for (int i = 0; i < 3; i++)//몬스터와 플레이어사이 거리를 계산하는루프
                    {
                        distance[i] = monster.transform.position - Player[i].transform.position;
                        sqrdistance[i] = distance[i].sqrMagnitude;
                    }

                    Min = sqrdistance[0];

                    for (int i = 0; i < 3; i++)//최소 거리를 Min에 집어넣기
                    {
                        if (sqrdistance[i] <= Min)
                        {
                            Min = sqrdistance[i];
                            player_monster_chased = Player[i];//최소거리를 갖는 플레이어를 지칭해주기
                        }
                    }
                    targetsetting(player_monster_chased);
                    monster.transform.LookAt(targetPosition);//지칭된 플레이어를 쫓아가기
                    monster.transform.Translate(Vector3.forward * 0.08f);
                    anime.SetBool("mon_walk", true);
                }


            }
        }
    }

    private void OnTriggerExit(Collider coll2)
    {
        if (coll2.tag == "Player")
        {
            count_player_entered--;
            if(count_player_entered == 0)
            {
                anime.SetBool("mon_walk", false);
            }

        }

        if (coll2.tag == "pmeat")
        {
            thereismeat = false;
            anime.SetBool("mon_run", false);
        }
    }

    private void targetsetting(GameObject target)
    {
        targetPosition = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
    }

}
