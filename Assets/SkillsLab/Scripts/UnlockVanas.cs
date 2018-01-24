using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockVanas : MonoBehaviour {

    public SwitchPanels switcher;
    const string BADGETAG = "badge";
    const string WRISTBANDTAG = "wristband";

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == BADGETAG)
        {
            switcher.Login();
        }
        else if (other.tag == WRISTBANDTAG)
        {
            switcher.ShowResult(other.gameObject.GetComponent<Wristband>().patientData.patient);
        }
    }
}
