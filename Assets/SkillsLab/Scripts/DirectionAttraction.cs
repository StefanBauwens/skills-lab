using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionAttraction : MonoBehaviour
{
    const string TAGSNAP = "injectionZone";

    protected bool isColliding;
    protected Transform injectionZone; //an injection zone the syringe currently is colliding with
    protected PullSyringe pullSyringe;
    protected Collider _collidingObject;

    public bool IsCollidingWithInjectionZone
    {
        get{
            return isColliding;
        }
    }

    public Collider CollidingObject
    {
        get{
            return _collidingObject;
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
            _collidingObject = other;
            if (other.gameObject.GetComponent<Human>()) //if the injectionzone is part of human, only then you need to select injectiontype
            {
                if (other.transform.parent.parent.gameObject.GetComponent<PatientPerson>().patient.Equals(Tracker.patient))
                {
                    Tracker.interactedWithCorrectPatient = true;
                }
                else
                {
                    Tracker.wrongPatient++;
                }
                pullSyringe.SelectInjectionMethod();           
            }
            else
            {
                pullSyringe.HasChosen = true;
                pullSyringe.CurrentlCollidingMedicine = other.transform.parent.gameObject.GetComponent<MedicineData>().medicine;
            }
            isColliding = true;
            injectionZone = other.gameObject.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == TAGSNAP)
        {
            _collidingObject = null;
            isColliding = false;
            pullSyringe.HasChosen = false;
            pullSyringe.ObjectIsHuman = false;
        }
    }

}
