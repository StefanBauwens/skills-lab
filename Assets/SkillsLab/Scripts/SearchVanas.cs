using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SearchVanas : MonoBehaviour
{
    protected Patient[] patients; //represents a list of all patients
    //protected Medical[] medicals; //represents all the medical stuff to be found in the shelf
    protected Medicine[] medicals;

    // Use this for initialization
    public void Start()
    {
        //patients = new Patient[] { new Patient("Bauwens", "Stefan", "jabla", 22, true, 62.5f, "not aware of any", "none", "Effervescent tablet", "x", "2 Packs of effervecent tablets per day"), new Patient("Ho", "Cindy", "habla", 20, false, 1, "Air", "none", "Effervescent tablet", "x", "1 shot of cocaine a day") };
		/*medicals = new Medical[] {
			new Medical ("Ibuprofen", "", "A1", TypeOfMedicine.Pill, 4),
			new Medical ("Aspirine", "bruis", "B2", TypeOfMedicine.EffervescentTablet, 3),
			new Medical ("Fun", "bla", "C#", TypeOfMedicine.IDKYET, 9001)

		};*/
    //THERE IS CURRENTLY NOTHING IN VANAS DATABASE. SEE WITH XML

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

    /*public Medical[] SearchForMedical(string name)
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
    }*/

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