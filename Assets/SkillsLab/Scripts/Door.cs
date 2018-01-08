using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        anim.SetBool("open", true);
        Debug.Log("Open door");
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("close", true);
        Debug.Log("Close door");
    }

    public void ResetBoolean(string doorState)
    {
        if(doorState == "opened")
        {
            anim.SetBool("open", false);
            Debug.Log("open bool reset");
        }
        else if(doorState == "closed")
        {
            anim.SetBool("close", false);
            Debug.Log("close bool reset");
        }   
    }
}
