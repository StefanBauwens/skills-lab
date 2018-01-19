using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By Stefan
//This script will look in XMLData for the Patient from the current scenario and load it. 

/// <summary>
/// OBSOLETE! THIS SCRIPT WILL NOT BE USED. 
/// </summary>


public class LoadPatient : MonoBehaviour {
    public GameObject adultMan; //prefab of adult man
    public GameObject child;
    public GameObject pregnantWoman;
    public GameObject seniorMan;

    protected Patient patientData;
    protected GameObject patient;

	// Use this for initialization
	void Start () {
        patientData = XMLData.appData.mPatients[XMLData.scenario.mPatientID];
        Debug.Log("Loading prefab of " + patientData.mType);
        switch (patientData.mType)
        {
            case PatientType.adult:
                patient = Instantiate(adultMan, this.transform.position, this.transform.rotation);
                break;
            case PatientType.child:
                patient = Instantiate(child, this.transform.position, this.transform.rotation);
                break;
            case PatientType.pregnant:
                patient = Instantiate(pregnantWoman, this.transform.position, this.transform.rotation);
                break;
            case PatientType.senior:
                patient = Instantiate(seniorMan, this.transform.position, this.transform.rotation);
                break;
            default:
                Debug.Log("Error trying to load patient.");
                break;
        }
    }

}
