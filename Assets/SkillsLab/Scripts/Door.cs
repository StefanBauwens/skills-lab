using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private Animator anim;
	protected string state;

    private void Start()
    {
		//state = "opened";
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Head")
        {
            anim.SetBool("open", true);
        }
		
        Debug.Log("gameobject: " + gameObject.name + " other collider enter: " + other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Head")
        {
            anim.SetBool("open", false);
        }
            
        Debug.Log("gameobject: "+ gameObject.name + " other collider exit: " + other);
    }

    public void ResetBoolean(string doorState)
    {
		/*state = doorState;
        if(doorState == "opened")
        {
            anim.SetBool("open", false);
            Debug.Log("open bool reset");
        }
        else if(doorState == "closed")
        {
            anim.SetBool("close", false);
            Debug.Log("close bool reset");
        }   */
    }
}
