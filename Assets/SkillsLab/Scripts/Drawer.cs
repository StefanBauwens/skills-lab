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
    private Vector3 startPos;
    private bool opened;

    void Start()
    {
        //startPos = transform.position;
        interactScript = GetComponent<VRTK_InteractableObject>();
        rb = GetComponent<Rigidbody>();
        // Subscribe function to the event
        interactScript.InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
    }

    private void Update()
    {
        // Drawer gets locked when it's back in the start position
        //if(opened && transform.position == startPos)
        //{
        //    SetGrabStatus(false);
        //}
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
        Debug.Log("Parent Obj: " + gameObject.transform.parent);
        Debug.Log("Drawer: " + gameObject);
        if (enableGrab)
        {
            interactScript.isGrabbable = true;
        }
        else
        {
            interactScript.isGrabbable = false;
        }
    }

    // Rigidbody will detect/not detect other rigidbodies
    public void SetRigidbodyStatus(bool enableRb)
    {
        if (enableRb)
        {
            rb.detectCollisions = true;
        }
        else
        {
            rb.detectCollisions = false;
        }
    }

    // Called when object is grabbed
    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("Im Grabbed");
        SetLightStatus(false);
        opened = true;
    }

}
