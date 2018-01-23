using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//auto loads the gray vanas with meds. (check quantity) (max quantity) 
//IGNORES the quanitity given by xml


public class LoadGray : MonoBehaviour {
    public MedicinesPrefabs[] medicinePrefabs; //fill this in the inspector so the program knows what gameobjects to instantiate by certain medicines
    public GameObject unknownMedicinePrefab;

    const int MAXDRAWERS = 6;
    const int MAXCOMPORTMENTS = 6; //max quanitity
    const int MAXCOLS = 3;
    //each collumn has 6 drawers and each drawer has 5 compartments. However, unlike the blue shelf, in one drawer is only the same medicine.(just multiple times)

    const string DRAWERS = "Drawers";
    const string GRAYMED = "#GRAY";
	// Use this for initialization

    protected List<Compartments> compartments = new List<Compartments>(); //notice compartmentS, not compartment (see loadVanas.cs)
	void Start () {
        Transform drawersGameObject = this.transform.Find(DRAWERS);

        int index = 0;
        foreach (Transform column in drawersGameObject)
        {
            foreach (Transform drawer in column)
            {
                compartments.Add(new Compartments(index, drawer.GetComponentsInChildren<Compartment>()));
                index++;
            }
        }


        //spawn the objects
        index = 0;
        foreach (var med in XMLData.appData.mMedicines)
        {
            if (med.mName.ToLower().Contains(GRAYMED.ToLower()))
            {
                Debug.Log("Graymed found!");
                GameObject medicineObject = Array.Find(medicinePrefabs, x => (x.medicineName.ToLower() == med.mName.Split('#')[0].ToLower()) && (x.medicinePackage == med.mPackage)).medicinePrefab; //looks if the medicine is in the medicinePrefabs array so it can know which prefabs belongs with it
                if (medicineObject == null)
                {
                    medicineObject = unknownMedicinePrefab;
                }
                foreach (var compartment in compartments[index]._compartment)
                {
                    Instantiate(medicineObject, compartment.transform.position, medicineObject.transform.rotation);
                }
                compartments[index]._compartment[0].transform.parent.GetComponent<Drawer>().medicinesInDrawer.Clear();
                compartments[index]._compartment[0].transform.parent.GetComponent<Drawer>().medicinesInDrawer.Add(med.Name);
                index++;
                if (index+1 > compartments.Count)
                {
                    return;
                }
            }
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
