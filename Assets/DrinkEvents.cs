using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkEvents : MonoBehaviour {

    [HideInInspector]
    public GameObject cupObject;
    public GameObject handUsed; // Hand that will hold cup
    public Transform cupOnTablePos;
    public GameObject noClothing;
    

    public void CupFollowHand()
    {
        cupObject.transform.SetParent(handUsed.transform);
    }

    public void EmptyCup()
    {
        cupObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void CupOnTable()
    {
        Destroy(cupObject);
        //cupObject.transform.position = cupOnTablePos.position;
        //cupObject.transform.rotation = cupOnTablePos.rotation;
        //cupObject.GetComponent<Rigidbody>().useGravity = true;
        //cupObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    // Switch back to body with no clothing
    public void SwitchBodyNoClothing()
    {
        noClothing.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
