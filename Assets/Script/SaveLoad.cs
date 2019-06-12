using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad : MonoBehaviour
{
    public bool IsSave;
    [System.Serializable]
    public class Data
    {
        public float AplayerX;
        public float AplayerY;
        public float AplayerZ;
        public float A_HP;
        public float A_Full;
        public float A_scare;

        public float BplayerX;
        public float BplayerY;
        public float BplayerZ;
        public float B_HP;
        public float B_Full;
        public float B_scare;

        public float CplayerX;
        public float CplayerY;
        public float CplayerZ;
        public float C_HP;
        public float C_Full;
        public float C_scare;

        public List<bool> swList;
        public List<string> swNameList;
        public List<string> varNameList;
        public List<float> NumList;
    }

    private MoveCharacter CharA;
    private MoveCharacter CharB;
    private MoveCharacter CharC;
    private playerState AplayerState;
    private playerState BplayerState;
    private playerState CplayerState;

    public Data data;
    private DatabaseManager Database;
    private Vector3 vector;


    private void Start()
    {
        CharA = GameObject.Find("ACharacter").GetComponent<MoveCharacter>();
        CharB = GameObject.Find("BCharacter").GetComponent<MoveCharacter>();
        CharC = GameObject.Find("CCharacter").GetComponent<MoveCharacter>();

        AplayerState = GameObject.Find("ACharacter").GetComponent<playerState>();
        BplayerState = GameObject.Find("BCharacter").GetComponent<playerState>();
        CplayerState = GameObject.Find("CCharacter").GetComponent<playerState>();
    }

    public void CallSave()
    {
        /* 일단은 start문에 선언해보자. 
        CharA = GameObject.Find("ACharacter").GetComponent<MoveCharacter>();
        CharB = GameObject.Find("BCharacter").GetComponent<MoveCharacter>();
        CharC = GameObject.Find("CCharacter").GetComponent<MoveCharacter>();

        AplayerState = GameObject.Find("ACharacter").GetComponent<playerState>();
        BplayerState = GameObject.Find("BCharacter").GetComponent<playerState>();
        CplayerState = GameObject.Find("CCharacter").GetComponent<playerState>();
        */

        data.AplayerX = CharA.transform.position.x;
        data.AplayerY = CharA.transform.position.y;
        data.AplayerZ = CharA.transform.position.z;
        data.A_HP = AplayerState.hp;
        data.A_Full = AplayerState.full;
        data.A_scare = AplayerState.scare;
        
        data.BplayerX = CharB.transform.position.x;
        data.BplayerY = CharB.transform.position.y;
        data.BplayerZ = CharB.transform.position.z;
        data.B_HP =      BplayerState.hp;
        data.B_Full =    BplayerState.full;
        data.B_scare =   BplayerState.scare;

        data.CplayerX = CharC.transform.position.x;
        data.CplayerY = CharC.transform.position.y;
        data.CplayerZ = CharC.transform.position.z;
        data.C_HP = CplayerState.hp;
        data.C_Full = CplayerState.full;
        data.C_scare = CplayerState.scare;

        Debug.Log("데이터 성공");
        /*
        for(int i = 0; i< Database.var_name.Length; i++)
        {
            data.varNameList.Add(Database.var_name[i]);
            data.NumList.Add(Database.var[i]);
        }
        for (int i = 0; i < Database.switch_name.Length; i++)
        {
            data.swNameList.Add(Database.switch_name[i]);
            data.swList.Add(Database.switches[i]);
        }
        */

        BinaryFormatter bf = new BinaryFormatter(); //변환
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat");//임시확장자
        bf.Serialize(file, data); //파일에 기록.직렬화
        file.Close();

        Debug.Log(Application.dataPath + "에 저장완료.");
        IsSave = true;
    }

    public void CallLoad()
    {
        BinaryFormatter bf = new BinaryFormatter(); //변환
        FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open);

        if (file != null && file.Length > 0)
        {
            data = (Data)bf.Deserialize(file);

           
            vector.Set(data.AplayerX, data.AplayerY, data.AplayerZ);
            CharA.transform.position = vector;
            AplayerState.hp = data.A_HP;
            AplayerState.full = data.A_Full;
            AplayerState.scare = data.A_scare;

            vector.Set(data.BplayerX, data.BplayerY, data.BplayerZ);
            CharB.transform.position = vector;
            BplayerState.hp = data.B_HP;
            BplayerState.full = data.B_Full;
            BplayerState.scare = data.B_scare;

            vector.Set(data.CplayerX, data.CplayerY, data.CplayerZ);
            CharC.transform.position = vector;
            CplayerState.hp = data.C_HP;
            CplayerState.full = data.C_Full;
            CplayerState.scare = data.C_scare;

            /*
            Database.var = data.NumList.ToArray();
            Database.var_name = data.varNameList.ToArray();
            Database.switch_name = data.swNameList.ToArray();
            Database.switches = data.swList.ToArray();
            */
         

        }
        else
        {
            Debug.Log("저장된 세이브 파일이 없습니다.");
        }
        file.Close();
    }
}
