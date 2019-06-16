using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_range : MonoBehaviour
{
    GameObject Player;
    public GameObject monster;
    
    public float HP_char;

    Animator anime;
    public bool a = false;
    bool b = true;

    // Start is called before the first frame update
    void Start()
    {
        anime = monster.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        { Invoke("run_attack", 3f);// 3초뒤부터 공격실행하게 해서 시야들어오자마자 공격당하는것 방지
            if (a)
            { if (b)
                {
                    Player = coll.gameObject;
                }
                b = false;

            
                anime.SetTrigger("mon_attack");

                HP_char = Player.GetComponent<playerState>().hp;
                HP_char -= 20;
            }
        }

    }
    /*private void OnTriggerExit(Collider col2)
    {
        if (col2.tag == "Player")
        {
            b = true;
        }*/
    



    void run_attack()
    {
        a = true;
    }
}

