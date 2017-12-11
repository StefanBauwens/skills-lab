using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Item : MonoBehaviour {

    private VRTK_InteractableObject interactScript;
    private Rigidbody rb;
    public bool touchesTable;
    public bool isGrabbed;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        interactScript = GetComponent<VRTK_InteractableObject>();
        interactScript.InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
    }

    // Disable all constraints when item is grabbed
    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        rb.constraints = RigidbodyConstraints.None;
        transform.parent = null;
        isGrabbed = true;      
    }
}
