using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CheckDrawerEmpty : MonoBehaviour {

    private byte count;
    private Rigidbody rb;
    private VRTK_InteractableObject interactScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        interactScript = GetComponent<VRTK_InteractableObject>();
        interactScript.InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectUnGrabbed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Item>())
        {
            count++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Item>())
        {
            count--;
            if (count == 0) // Drawer is empty
            {
                LockDrawerEndPos.isLocked = false;
                // Drawer can move back to start position
                GetComponent<Drawer>().SetRigidbodyStatus(true);
                GetComponent<Drawer>().SetGrabStatus(true);
                this.enabled = false; // Disable this script
            }
        }
    }

    // Called when object(drawer) is ungrabbed
    private void ObjectUnGrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (LockDrawerEndPos.isLocked)
        {
            rb.WakeUp(); // Not on idle
            rb.isKinematic = true; // Stop moving of items
        }
    }
}
