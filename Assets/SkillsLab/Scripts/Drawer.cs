using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class Drawer : MonoBehaviour {

    public string medicineInDrawer;
    public GameObject drawerLight;
    private VRTK_InteractableObject interactScript;
    private Rigidbody rb;

    void Start()
    {
        interactScript = GetComponent<VRTK_InteractableObject>();
        rb = GetComponent<Rigidbody>();
        SetGrabStatus(false);
        SetRigidbodyStatus(false);
        
        // Subscribe function to the event
        interactScript.InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
    }

    public bool IsGrabbable
    {
        get{
            return interactScript.isGrabbable;
        }
    }


    // Enable/disable drawer light
    public void SetLightStatus(bool enableLight)
    {
        if (enableLight)
        {
            drawerLight.SetActive(true);
        }
        else
        {
            drawerLight.SetActive(false);
        }
    }

    // Make drawer grabbable/ungrabbable (lock)
    public void SetGrabStatus(bool enableGrab)
    {
        if (enableGrab)
        {
            interactScript.isGrabbable = true;
        }
        else
        {
            interactScript.isGrabbable = false;
        }
    }

    // Drawer will be locked (freeze pos & rot)
    public void SetRigidbodyStatus(bool unlockZ)
    {
        if (unlockZ) // Unfreeze pos z to unlock drawer
        {  
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
            rb.isKinematic = false; // Stop weird forces on items
        }
        else
        { 
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.isKinematic = true;
        }
    }

    // Called when object(drawer) is grabbed
    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        //SetLightStatus(false); --> turn off when start pos reached
    }

}
