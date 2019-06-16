using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{  // Start is called before the first frame update
    public GameObject exit_messege;
    public GameObject exit_out;
    public GameObject exit_stay;
    void Start()
    {
        exit_messege.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            exit_messege.SetActive(true);
            //give a message ui 
        }
    }

    public void Click_exit()
    {
        exit_messege.SetActive(false);
        SceneManager.LoadScene("End_Sc"); //탈출성공! finish ending
    }

    public void Click_stay()
    {
        exit_messege.SetActive(false);
        //창 내리기 
    }

}
