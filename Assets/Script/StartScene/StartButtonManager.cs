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
        File.Delete(Application.dataPath + "/SaveFile.dat");
        File.Delete(Application.dataPath + "/save/InventoryData.xml");
        SceneManager.LoadScene("Main");

    }

    public void OnClickLoad()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Main");


        Destroy(gameObject);
    }
}
