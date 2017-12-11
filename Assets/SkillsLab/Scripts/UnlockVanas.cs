using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockVanas : MonoBehaviour {

    public SwitchPanels switcher;
    const string BADGETAG = "badge";

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == BADGETAG)
        {
            switcher.Login();
        }
    }
}
