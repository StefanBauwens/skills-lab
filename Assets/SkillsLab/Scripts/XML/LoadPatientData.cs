using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//By Stefan. Used to attach a patient script to the patient gameobjects. 

public class LoadPatientData : MonoBehaviour {
    public PatientPerson adult; //IF HAVING ERROR CHECK IF THESE ARE FILLED IN. 
    public PatientPerson child;
    public PatientPerson pregnant;
    public PatientPerson senior;

    protected Patient patient;
    protected Patient unknownPatient;

    // Use this for initialization
    public void Start () {

        patient = XMLData.appData.mPatients[XMLData.scenario.mPatientID];

        switch (patient.mType)
        {
            case PatientType.adult:
                adult.patient = patient; //@FUTRURE STEFAN: If you get an error saying there is not Patientperson, just add a new script on the gameobject! You're welcome :)
                break;
            case PatientType.child:
                child.patient = patient;
                break;
            case PatientType.pregnant:
                pregnant.patient = patient;
                break;
            case PatientType.senior:
                senior.patient = patient;
                break;
            default:
                Debug.Log("Unknown patienttype... Fix XML");
                break;
        }
        adult.Start();
        child.Start();
        pregnant.Start();
        senior.Start();
	}
}
