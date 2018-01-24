using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkEvents : MonoBehaviour {

    [HideInInspector]
    public GameObject cupObject;
    public GameObject handUsed; // Hand that will hold cup
    public Transform cupOnTablePos;
    public GameObject noClothing;
	public SphereCollider snapCupRightColl;
    

    public void CupFollowHand()
    {
        cupObject.transform.SetParent(handUsed.transform);
        Debug.Log("parent cup: " + cupObject.transform.parent);
    }

    public void EmptyCup()
    {
        if (cupObject.transform.GetChild(0).gameObject.activeSelf)
        {
            if (Tracker.patient == this.transform.parent.GetComponent<PatientPerson>().patient)
            {
                Tracker.interactedWithCorrectPatient = true;
            }
            else
            {
                Tracker.wrongPatient++;
            }
        }
        cupObject.transform.GetChild(0).gameObject.SetActive(false);

    }

    public void CupOnTable()
    {
		cupObject.transform.SetParent (null);
		snapCupRightColl.enabled = true;
		cupObject.GetComponent<Rigidbody>().useGravity = false;
		cupObject.GetComponent<Rigidbody>().isKinematic = true;

		cupObject.transform.position = cupOnTablePos.position;
		cupObject.transform.rotation = cupOnTablePos.rotation;
        cupObject.GetComponent<Rigidbody>().useGravity = true;
        cupObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    // Switch back to body with no clothing
    public void SwitchBodyNoClothing()
    {
        noClothing.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
