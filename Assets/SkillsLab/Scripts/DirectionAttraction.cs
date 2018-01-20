using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionAttraction : MonoBehaviour
{
    const string TAGSNAP = "injectionZone";

    protected bool isColliding;
    protected Transform injectionZone; //an injection zone the syringe currently is colliding with
    protected PullSyringe pullSyringe; 

    public bool IsCollidingWithInjectionZone
    {
        get{
            return isColliding;
        }
    }

    // Use this for initialization
    void Start()
    {
        isColliding = false;
        pullSyringe = this.transform.parent.GetComponent<PullSyringe>();
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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TAGSNAP)
        {
            isColliding = true;
            injectionZone = other.gameObject.transform;
            pullSyringe.SelectInjectionMethod();           
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == TAGSNAP)
        {
            isColliding = false;
            pullSyringe.HasChosen = false;
        }
    }

}
