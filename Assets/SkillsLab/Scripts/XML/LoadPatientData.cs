using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//By Stefan. Used to attach a patient script to the patient gameobjects. 

public class LoadPatientData : MonoBehaviour {
    public GameObject adult; //IF HAVING ERROR CHECK IF THESE ARE FILLED IN. 
    public GameObject child;
    public GameObject pregnant;
    public GameObject senior;

    protected Patient patient;
    protected Patient unknownPatient;

    // Use this for initialization
    void Start () {

        patient = XMLData.appData.mPatients[XMLData.scenario.mPatientID];

        switch (patient.mType)
        {
            case PatientType.adult:
                adult.GetComponent<PatientPerson>().patient = patient; //@FUTRURE STEFAN: If you get an error saying there is not Patientperson, just add a new script on the gameobject! You're welcome :)
                break;
            case PatientType.child:
                child.GetComponent<PatientPerson>().patient = patient;
                break;
            case PatientType.pregnant:
                pregnant.GetComponent<PatientPerson>().patient = patient;
                break;
            case PatientType.senior:
                senior.GetComponent<PatientPerson>().patient = patient;
                break;
            default:
                Debug.Log("Unknown patienttype... Fix XML");
                break;
        }
	}
}
