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
        if (File.Exists(Application.dataPath + "/Save/InventoryData.xml"))
            File.Delete(Application.dataPath + "/Save/InventoryData.xml");
        if (File.Exists(Application.dataPath + "/Save/ItemGetData.xml"))
            File.Delete(Application.dataPath + "/Save/ItemGetData.xml");

        SceneManager.LoadScene("Main");

    }

    public void OnClickLoad()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Main");


        Destroy(gameObject);
    }
}
