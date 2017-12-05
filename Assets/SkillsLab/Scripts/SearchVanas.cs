using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SearchVanas : MonoBehaviour
{
    protected Patient[] patients; //represents a list of all patients
    protected Medical[] medicals; //represents all the medical stuff to be found in the shelf

    // Use this for initialization
    public void Start()
    {
        patients = new Patient[] { new Patient("Bauwens", "Stefan", "jabla", 22, true), new Patient("Ho", "Cindy", "habla", 20, false) };
        medicals = new Medical[] { new Medical("Ibuprofen", "", "A1", TypeOfMedicine.Pill, 4), new Medical("Asperine", "bruis", "B2", TypeOfMedicine.EffervescentTablet, 3), new Medical("Fun", "bla", "C#", TypeOfMedicine.IDKYET, 9001), new Medical("Syringe 1.5ml", "", "", TypeOfMedicine.Syringe, 1), new Medical("Syringe 2.5ml", "", "", TypeOfMedicine.Syringe, 1), new Medical("Syringe 5ml", "", "", TypeOfMedicine.Syringe, 1), new Medical("Syringe 10ml", "", "", TypeOfMedicine.Syringe, 1), /*new Medical("Syringe 20ml", "", "", TypeOfMedicine.Syringe, 1),*/ new Medical("Syringe 50ml", "", "", TypeOfMedicine.Syringe, 1) };
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
                if (patient.FirstName.ToLower().Contains(fName.ToLower()) && !results.Contains(patient))
                {
                    results.Add(patient);
                }
                if (patient.Name.ToLower().Contains(lName.ToLower()) && !results.Contains(patient))
                {
                    results.Add(patient);
                }
            }
        }
        return results.ToArray();
    }

    public Medical[] SearchForMedical(string name)
    {
        List<Medical> results = new List<Medical>();
        foreach (Medical medical in medicals)
        {
            if (medical.Name.ToLower().Contains(name.ToLower()))
            {
                results.Add(medical);
            }
        }
        return results.ToArray();
    }



}