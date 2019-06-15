using System.Collections;
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
    bool Is_monster_hungry;
    bool Is_monster_weaken = false;

    public Vector3 targetPosition; //쫓을 대상(밑에targetsetting함수로 정함)

    Animator anime;

    public float temp;
    public GameObject attack_range;
    Transform player_position;
    bool b = true;//


    // Start is called before the first frame update
    void Start()
    {
        anime = gameObject.GetComponentInParent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        temp = Vector3.Distance(monster.transform.position, poisened_meat.transform.position);
        Is_monster_hungry = gameObject.GetComponentInParent<Monster_chasing>().mon_empty_stomach;
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
            if (Is_monster_hungry)
            {
                thereismeat = true;
            }
        }
    }


    private void OnTriggerStay(Collider coll)
    {
        bool a = true;//독고기 사라져도 thereismeat 다시 true되는 경우 방지 false면 독고기 파괴됐다는거

        
        if (coll.tag == "pmeat") //시야에들어온 물체가 독고기일때
        {
            if (Is_monster_hungry&&a) // 공복상태여야함
            {
                thereismeat = true; //이미 시야에 독고기 들어오고 난다음 공복상태로 바뀐경우위해 여기에도 추가

                Targetsetting(poisened_meat);
                GameObject.FindGameObjectWithTag("monster").transform.LookAt(targetPosition);
                GameObject.FindGameObjectWithTag("monster").transform.Translate(Vector3.forward * 0.05f);
                anime.SetBool("mon_run", true);

                if (Vector3.Distance(monster.transform.position, poisened_meat.transform.position) < 3)
                {
                    a = false;
                   /* attack_range.transform.LookAt(poisened_meat.transform);
                    anime.SetTrigger("mon_attack");*/

                    Destroy(poisened_meat, 2.3f);//독고기 먹어서 없어진걸 나타냄
                    thereismeat = false;
                    StartCoroutine("Mon_weaken");//독고기 먹고 깜박거리면서 약화상태
                    Is_monster_weaken = true;
                    anime.SetBool("mon_run", false);
                }
            }

        }


        if ((coll.tag == "Player")&&!(thereismeat)) //시야에 들어온 물체가 플레이어일때 
        {
            if (count_player_entered == 1) //한명일 때
            {
                Targetsetting(coll.gameObject);
                monster.transform.LookAt(targetPosition);
                monster.transform.Translate(Vector3.forward * 0.05f);


                if (Vector3.Distance(monster.transform.position, coll.gameObject.transform.position) < 5)

                {//거리가 가까워지면 공격발동
                    
                    anime.SetBool("mon_walk", false);
                    if (b)
                    {
                        attack_range.transform.position = new Vector3(coll.gameObject.transform.position.x, attack_range.transform.position.y, coll.gameObject.transform.position.z);
                    }
                    b = false;//위에 이프조건문 한번만 실행하게 하려고
                    attack_range.GetComponent<SkinnedMeshRenderer>().enabled = true;
                    attack_range.GetComponent<CapsuleCollider>().enabled = true;

                    player_position.position = new Vector3(coll.gameObject.transform.position.x, attack_range.transform.position.y, coll.gameObject.transform.position.z);

                    attack_range.transform.LookAt(player_position);
                    attack_range.transform.Translate(Vector3.forward * 0.08f);
                    
                    
                }
                else
                {
                    attack_range.GetComponent<SkinnedMeshRenderer>().enabled = false;
                    attack_range.GetComponent<CapsuleCollider>().enabled = false;
                    b = true;
                   
                }

            }
                /*if (Is_monster_weaken)
                {
                    if (Vector3.Distance(monster.transform.position, coll.gameObject.transform.position) < 3)
                    {
                        Die();
                    }*/
        

            else //한명 이상일때
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
                Targetsetting(player_monster_chased);
                monster.transform.LookAt(targetPosition);//지칭된 플레이어를 쫓아가기
                monster.transform.Translate(Vector3.forward * 0.05f);
                anime.SetBool("mon_walk", true);

                if (Vector3.Distance(monster.transform.position, player_monster_chased.transform.position) < 3)
                {
                    anime.SetBool("mon_walk", false);
                    anime.SetBool("mon_attack", true);
                }
                else
                {
                    anime.SetBool("mon_attack", false);
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
                if (count_player_entered == 0)
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

    void Targetsetting(GameObject target)
    {
        targetPosition = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
    }



    IEnumerator Mon_weaken()//몬스터 약화상태일때 깜박깜박거리기
    {
        int counttime = 0;

        yield return new WaitForSeconds(3f);

        while (counttime < 10)
        {
            if (counttime % 2 == 0)
                monster.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            else
                monster.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;

            yield return new WaitForSeconds(0.2f);

            counttime++;
        }
        monster.GetComponent<Renderer>().enabled = true;

        yield return null;

    }

    

}


