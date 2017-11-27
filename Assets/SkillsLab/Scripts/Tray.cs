using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tray : MonoBehaviour {
    //makes items that are put in the tray child of the tray so that you can teleport and move around with it

    protected List<GameObject> children = new List<GameObject>();
    public Collider[] includeColliders;

    private void Start()
    {
        this.transform.parent.gameObject.GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!includeColliders.Contains(other)) //if it's not a collider that is in the include list
        {
            return;
        }

        if (!children.Contains(other.gameObject)) //if something collides with the the trigger collider of the tray (so it's in the tray) and it's not already child
        {
            children.Add(other.gameObject);
            other.gameObject.transform.parent = this.gameObject.transform; //changes the parent of the object to this
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (children.Contains(other.gameObject))
        {
            children.Remove(other.gameObject);
            other.gameObject.transform.parent = null; //changes the parent to none
        }
    }
}
