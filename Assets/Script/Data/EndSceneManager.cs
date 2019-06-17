using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        File.Delete(Application.dataPath + "/SaveFile.dat");
        if (File.Exists(Application.dataPath + "/InventoryData.xml"))
            File.Delete(Application.dataPath + "/InventoryData.xml");
        if (File.Exists(Application.dataPath + "/ItemGetData.xml"))
            File.Delete(Application.dataPath + "/ItemGetData.xml");

        Debug.Log("delete file succeed");
    }
    public void OnClickReplay()
    {
        SceneManager.LoadScene("Start");
    }

    public void OnClickEnd()
    {
        Application.Quit();
    }
}
