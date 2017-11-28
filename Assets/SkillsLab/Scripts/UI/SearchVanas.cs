using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SearchVanas : MonoBehaviour {
    protected Patient[] patients; //represents a list of all patients
    protected Medical[] medicals; //represetns all the medical stuff to be found in the shelf

	// Use this for initialization
	void Start () {
	}

    Patient[] SearchForName(string fName, string lName)
    {
        List<Patient> results = new List<Patient>();

        foreach (var patient in patients)
        {
            if (patient.FirstName == fName && patient.LastName == lName ) //if it finds a direct match exit (will not work if you have more than one member with the same first and lastname)
            {
                results.Clear();
                results.Add(patient);
                break;
            }
            else
            {
                if (patient.FirstName == fName)
                {
                    results.Add(patient);
                }
                if (patient.LastName == lName)
                {
                    results.Add(patient);
                }
            }
        }
        return results.ToArray();    
    }

    /*Medical[] SearchForMedical(string name)
    {
        foreach (var medical in medicals)
        {
            if (medical.Name = name)
            {

            }
        }
    }*/



}
