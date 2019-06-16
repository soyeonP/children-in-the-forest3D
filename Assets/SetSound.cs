using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSound : MonoBehaviour
{
    public GameObject inven;
    public GameObject making;
    public GameObject click;
    public GameObject click2;

    public AudioClip uiSound_1;
    public AudioClip uiSound_2;
    public AudioClip uiSound_3;
    public AudioClip mapSound;
    public AudioClip makingSound;
    public AudioClip trap;


    private AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setInvenSd()
    {
        audio.clip = uiSound_1;
        audio.Play();
    }
    public void setMakingSd()
    {
        audio.clip = uiSound_1;
        audio.Play();
    }
    public void setHuntSd()
    {
        audio.clip = uiSound_3;
        audio.Play();
    }
    public void setCheckSd()
    {
        audio.clip = uiSound_2;
        audio.Play();
    }
}
