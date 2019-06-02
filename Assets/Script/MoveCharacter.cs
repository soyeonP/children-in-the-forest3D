using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCharacter : MonoBehaviour {

 //   public Transform Target; 
 //   public bool Active = true;
     public float moveSpeed = 5f;
     public float turnSpeed = 5f;
     public bool join = false; // true : 조인 진행중 false : 조인끝 or 조인상태x
      public bool canMove = false;
    public GameObject Together;
    
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private Animator ani;

    public Vector3 position;

    /*save load 용 */
    private SaveLoad saveNLoad; 

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
      

        saveNLoad = FindObjectOfType<SaveLoad>();
        //game start 누르면 load하기 
        position = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F5))
        {
            saveNLoad.CallSave();//save data
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            saveNLoad.CallLoad();//save data
            position = transform.position;
        }
        float fDistance = Vector3.Distance(transform.position, position);

        if (fDistance > 1.5f)
        {



            Vector3 vec = (position - transform.position).normalized;
            Vector3 fixEulers = Quaternion.LookRotation(vec).eulerAngles; //회전값 범위내로 변경 
            fixEulers.x = 0f;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(fixEulers), turnSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            //Add Move ani
            ani.SetBool("Walk", true);
        }
        else
            ani.SetBool("Walk", false);



        /* else
         {
             //stay : Idle ani 
         }
         */


    }
}
