using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionAttraction : MonoBehaviour
{
    const string TAGSNAP = "injectionZone";

    protected bool isColliding;
    protected Transform injectionZone; //an injection zone the syringe currently is colliding with
    protected PullSyringe pullSyringe; 

    //protected Vector3 differenceRotationParent;

    // Use this for initialization
    void Start()
    {
        isColliding = false;
        pullSyringe = this.transform.parent.GetComponent<PullSyringe>();
        //differenceRotationParent = transform.eulerAngles - transform.parent.eulerAngles; //gives the difference between the parent and this
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding && pullSyringe.IsGrabbedWithNeedle)
        {
            gameObject.transform.LookAt(injectionZone, Vector3.down); //lookat rotates the transform so the FORWARD vector faces the quad.(Z axis)
        }
        else
        {
            this.transform.localEulerAngles = new Vector3(90, 0, 0);
        }

        /*if (transform.parent.parent == null) //if you aren't grabbing the syringe rotate the parent(capsule) so it has the same angle as this gameobject(top)
        {
            //TRY THIS AT SCHOOL:
            Vector3 differenceRotation = transform.parent.eulerAngles + differenceRotationParent - transform.eulerAngles;
            transform.parent.eulerAngles -= differenceRotation; //sets the parent to correct angle
            transform.eulerAngles += differenceRotation; //changes the child back to original rotation
        }*/
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TAGSNAP)
        {
            isColliding = true;
            injectionZone = other.gameObject.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == TAGSNAP)
        {
            isColliding = false;
        }
    }

    //WHEN STOP GRIPPING SYRINGE OR ON COLLIDER EXIT
}
