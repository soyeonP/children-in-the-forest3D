using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointoTogether : MonoBehaviour
{
    public Vector3 position;
    public bool join = false;
    private SaveLoad saveNLoad;

    // Start is called before the first frame update
    void Start()
    {
        saveNLoad = FindObjectOfType<SaveLoad>();

      
               saveNLoad.CallLoad();//save data
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (join == true)
        {
            
        }

    }

    void setPosition()
    {
        // 캐릭터를 canMove키고, 이동값은 transform.position의 위아래옆 으로 모인다.
      //  join = false;
        // 위 아래 옆 위치로 도착하면 canMove를 끄고 
        //그 위치에 together의 자식화 
        //그리고 together의 movecharacter의 canmove를 킨다. 
    }
}


