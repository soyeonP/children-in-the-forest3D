using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    public GameObject CharA;
    public GameObject CharB;
    public GameObject CharC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CharA.activeSelf == false&& CharB.activeSelf == false && CharC.activeSelf == false)
        {
            SceneManager.LoadScene("End_fail");
        }
    }
}
