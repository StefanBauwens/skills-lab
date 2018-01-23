using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By Stefan
//This script is neccesary since a patient class cannot be attached to a gameobject as it doesn't derive from Monobehaviour as this conflicts with XML loading.
//Instead this script is attached.
public class PatientPerson : MonoBehaviour {
    public Patient patient;

    public void Reset()
    {
        patient = new Patient();
    }
}
