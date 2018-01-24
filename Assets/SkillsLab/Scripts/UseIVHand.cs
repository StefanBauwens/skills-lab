using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class UseIVHand : MonoBehaviour {
    protected VRTK_SnapDropZone snapDropZone;

    void Start()
    {
        snapDropZone = this.gameObject.GetComponent<VRTK_SnapDropZone>();

        snapDropZone.ObjectSnappedToDropZone += new SnapDropZoneEventHandler(IVHandConnected);
    }

    protected void IVHandConnected(object sender, SnapDropZoneEventArgs e)
    {
        if (Tracker.patient == this.transform.parent.parent.gameObject.GetComponent<PatientPerson>().patient)
        {
            Tracker.interactedWithCorrectPatient = true;
        }
        else
        {
            Tracker.wrongPatient++;
        }
    }
}
