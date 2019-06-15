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
