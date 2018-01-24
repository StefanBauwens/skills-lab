using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DestroyMedicine : MonoBehaviour {

	protected VRTK_SnapDropZone snapDropZone;
	const string TAG = "medicine";

    void Start()
    {
        snapDropZone = this.gameObject.GetComponent<VRTK_SnapDropZone>();

        snapDropZone.ObjectSnappedToDropZone += new SnapDropZoneEventHandler(DestroyMed);
    }

	protected void DestroyMed(object sender, SnapDropZoneEventArgs e)
	{
        bool correctPatient = false;
        if (Tracker.patient.Equals(this.transform.parent.parent.gameObject.GetComponent<PatientPerson>().patient))
        {
            Tracker.interactedWithCorrectPatient = true;
            correctPatient = true;
        }
        else
        {
            Tracker.wrongPatient++;
        }

		foreach (Transform child in this.transform) {
			if (child.tag == TAG) {
                if (correctPatient && child.gameObject.GetComponent<MedicineData>().medicine.Equals(Tracker.medicine))
                {
                    Tracker.correctMedicineGiven = true; //if correctPatient and correct medicine
                    Tracker.quantityApplied++;
                }
                Destroy (child.gameObject);
			}
		}
    }
}
