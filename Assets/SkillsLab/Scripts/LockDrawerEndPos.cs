using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDrawerEndPos : MonoBehaviour {

    public static bool isLocked;
    protected const float SECONDSTOUNLOCK = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Drawer>())
        {
            isLocked = true;
            // Lock drawer at end position  
            other.GetComponent<Drawer>().SetGrabStatus(false); // Can't grab so that objects colliders won't move
            other.GetComponent<Drawer>().SetRigidbodyStatus(false); // Freeze all axis 
            other.GetComponent<CheckDrawerEmpty>().enabled = true; // Enable script component  
            StartCoroutine(Unlock(other.GetComponent<Drawer>()));
        }
    }

    IEnumerator Unlock(Drawer drawer)
    {
        yield return new WaitForSeconds(SECONDSTOUNLOCK);
        isLocked = false;
        drawer.SetGrabStatus(true);
        drawer.SetRigidbodyStatus(true);
        drawer.gameObject.GetComponent<CheckDrawerEmpty>().enabled = false;
    }
}
