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
    private ConfigurableJoint configJoint;
    private bool opened;

    void Start()
    {
        interactScript = GetComponent<VRTK_InteractableObject>();
        rb = GetComponent<Rigidbody>();
        configJoint = GetComponent<ConfigurableJoint>();
        SetGrabStatus(false);
        SetRigidbodyStatus(false);
        
        // Subscribe function to the event
        interactScript.InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
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

    // Enable/disable opening the drawer
    public void SetGrabStatus(bool enableGrab)
    {
        if (enableGrab)
        {
            Debug.Log("Parent Obj: " + gameObject.transform.parent);
            Debug.Log("Drawer: " + gameObject);
            interactScript.isGrabbable = true;
        }
        else
        {
            interactScript.isGrabbable = false;
        }
    }

    // Drawer will be locked (freeze pos & rot)
    public void SetRigidbodyStatus(bool enableRb)
    {
        if (enableRb) // Won't freeze pos z for opening drawer
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void SetConfigJointStatus(bool enableMotion)
    {
        if (enableMotion)
        {
            configJoint.xMotion = ConfigurableJointMotion.Limited;
        }
        else
        {
            configJoint.xMotion = ConfigurableJointMotion.Locked;
            Debug.Log("locked motion");
        }
    }

    // Called when object is grabbed
    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("Im Grabbed");
        //SetLightStatus(false);
        opened = true;
    }

}
