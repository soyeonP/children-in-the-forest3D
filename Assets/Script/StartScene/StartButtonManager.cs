using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButtonManager : MonoBehaviour
{
    private  bool isSaved = false;

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        if (File.Exists(Application.dataPath + "/SaveFile.dat")) isSaved = true;
        else
        {
            isSaved = false;
            GameObject.Find("Load").GetComponent<Button>().enabled = false;
        }
    }
    public void OnClickNewStart()
    {
        PlayerPrefs.SetInt("isGotsling", 1);
        PlayerPrefs.SetInt("isGotmushroom_soup", 1);
        PlayerPrefs.SetInt("isGotpoison_meat", 0);

        File.Delete(Application.dataPath + "/SaveFile.dat");
        if (File.Exists(Application.dataPath + "/InventoryData.xml"))
            File.Delete(Application.dataPath + "/InventoryData.xml");
        if (File.Exists(Application.dataPath + "/ItemGetData.xml"))
            File.Delete(Application.dataPath + "/ItemGetData.xml");

        SceneManager.LoadScene("Main");

    }

    public void OnClickLoad()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Main");


        Destroy(gameObject);
    }
}
