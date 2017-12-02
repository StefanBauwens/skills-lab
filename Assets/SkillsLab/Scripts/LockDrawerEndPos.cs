using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDrawerEndPos : MonoBehaviour {

    public static bool isLocked;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Drawer>())
        {
            isLocked = true;
            // Lock drawer at end position  
            other.GetComponent<Drawer>().SetGrabStatus(false); // Can't grab so that objects colliders won't move
            other.GetComponent<Drawer>().SetRigidbodyStatus(false); // Freeze all axis 
            other.GetComponent<CheckDrawerEmpty>().enabled = true; // Enable script component  
        }
    }
}
