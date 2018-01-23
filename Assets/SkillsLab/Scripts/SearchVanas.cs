using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SearchVanas : MonoBehaviour
{
    protected Patient[] patients; //represents a list of all patients
    protected List<Medicine> medicals = new List<Medicine>();
    const string GRAYMED = "#GRAY";

    // Use this for initialization
    public void Start()
    {
        patients = XMLData.appData.mPatients.ToArray();

        medicals = XMLData.GetMedicinesFromScenario(XMLData.scenario);

        foreach (var medicine in XMLData.appData.mMedicines) //add gray objects in vanas
        {
            if (medicine.mName.ToLower().Contains(GRAYMED.ToLower()))
            {
                medicals.Add(medicine.CleanUpName());
            }
        }
    }

    public Patient[] SearchForName(string fName, string lName)
    {
        List<Patient> results = new List<Patient>();
        results.Clear();
        foreach (Patient patient in patients)
        {
            if (patient.FirstName.ToLower() == fName.ToLower() && patient.Name.ToLower() == lName.ToLower()) //if it finds a direct match exit (will not work if you have more than one member with the same first and lastname)
            {
                results.Clear();
                results.Add(patient);
                break;
            }
            else
            {
                if (patient.FirstName.ToLower().Contains(fName.ToLower())&& fName.Length > 0)
                {
                    results.Add(patient);
                }
                if (patient.Name.ToLower().Contains(lName.ToLower()) && lName.Length > 0)
                {
                    results.Add(patient);
                }
            }
        }
        return results.ToArray();
    }

    public Medicine[] SearchForMedical(string name)
    {
        List<Medicine> results = new List<Medicine>();
        foreach (Medicine medical in medicals)
        {
            if (medical.Name.ToLower().Contains(name.ToLower()))
            {
                results.Add(medical);
            }
        }
        return results.ToArray();
    }

}