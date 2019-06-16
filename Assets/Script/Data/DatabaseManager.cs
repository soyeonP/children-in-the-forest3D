using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;
    public string[] var_name;
    public float[] var;

    public string[] switch_name; //몬스터나 오브젝트 저장용 
    public bool[] switches; // 몬스터or오브젝트 등장 true /false용 

    void Start()
    {
        
    }


}
