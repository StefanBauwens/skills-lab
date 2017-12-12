using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tray : MonoBehaviour {
    //makes items that are put in the tray child of the tray so that you can teleport and move around with it

    protected List<GameObject> children = new List<GameObject>();
    public Collider[] includeColliders;
    public int detachWhenCollidingWithLayer;
    public float breakTime;
    protected float beginTime;
    protected bool isColliding;
    protected List<Collider> CollidingObjects;

    private void Start()
    {
        this.transform.parent.gameObject.GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
        isColliding = false;
        CollidingObjects = new List<Collider>();
    }

    private void Update()
    {
        if (isColliding)
        {
            if (Time.time-beginTime > breakTime && this.transform.parent.parent != null)
            {
                this.transform.parent.parent = null; //get loose from controller
                this.transform.parent.GetComponent<Rigidbody>().isKinematic = false;
                isColliding = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!includeColliders.Contains(other)) //if it's not a collider that is in the include list
        {
            if (this.transform.parent.parent != null && detachWhenCollidingWithLayer == other.gameObject.layer) //if tray is grabbed
            {
                isColliding = true;
                beginTime = Time.time;
                CollidingObjects.Add(other);
            }
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
        else if (CollidingObjects.Contains(other))
        {
            CollidingObjects.Remove(other);
            if (CollidingObjects.Count == 0)
            {
                isColliding = false;
            }
        }
    }
}
