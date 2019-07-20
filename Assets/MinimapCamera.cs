using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private Vector3 pos;
    public GameObject[] players;
    int moveChild;

    // Start is called before the first frame update
    void Start()
    {
        pos = new Vector3(0, transform.position.y, 0);
        moveChild = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = pos;
        transform.position = players[moveChild - 1].transform.position + new Vector3(0, 500, 0);
    }

    public void SetMoveChild(int movechild)
    {
        this.moveChild = movechild;
    }
}
