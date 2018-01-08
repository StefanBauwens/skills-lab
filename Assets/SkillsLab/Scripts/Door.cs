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
		//if (state=="closed") { //if isn't already opening and is closed
			anim.SetBool ("open", true);
			Debug.Log ("Open door");
		//}
    }

    private void OnTriggerExit(Collider other)
    {
		//if (state=="opened") { //if isn't already closing and is open
		//	anim.SetBool("close", true);
		anim.SetBool("open", false);
			Debug.Log("Close door");
		//}

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
