using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By Stefan
//This script is neccesary since a patient class cannot be attached to a gameobject as it doesn't derive from Monobehaviour as this conflicts with XML loading.
//Instead this script is attached.
public class PatientPerson : MonoBehaviour {
    public Patient patient = new Patient();
    public string name;

    public void Start()
    {
        StartCoroutine(ChangeName());
    }

    IEnumerator ChangeName()//this is only for debugging puproses and may be removed at a later time
    {
        yield return new WaitForSeconds(2); 
        name = patient.FirstName;
    }
}
