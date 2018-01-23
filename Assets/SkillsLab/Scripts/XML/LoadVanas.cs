using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Linq;

//By Stefan
//UP to 5 items in one drawer of a blue vanas shelf (because 5 physcial compartments) and max 6 drawers
//This script is placed on the BlueVanas
[Serializable]
public struct MedicinesPrefabs
{
    public string medicineName;
    public Package medicinePackage;
    public GameObject medicinePrefab;
}

public struct Compartments
{
    public int _drawerID;
    public Compartment[] _compartment;

    public Compartments(int drawerID, Compartment[] compartment)
    {
        _drawerID = drawerID;
        _compartment = compartment;
    }
}

public class LoadVanas : MonoBehaviour {
    public MedicinesPrefabs[] medicinePrefabs; //fill this in the inspector so the program knows what gameobjects to instantiate by certain medicines
    public GameObject unknownMedicinePrefab;

    protected List<GameObject> instantiatedMeds = new List<GameObject>(); //keeps a list of medicines that are instantiated so it can remove them when loading a new scenario

    const int MAXDRAWERS = 6;
    const int MAXCOMPORTMENTS = 5;
    const string DRAWERS = "Drawers";
    protected List<Compartments> compartments = new List<Compartments>();

	// Use this for initialization
	public void Start () {
        Transform drawersGameObject = this.transform.Find(DRAWERS);
        int index = 0;

        foreach (var item in instantiatedMeds)
        {
            Destroy(item); //deletes all previous instantiated medicines
        }
        instantiatedMeds.Clear();

        foreach (Transform child in drawersGameObject) //makes a list of compartments
        {
            child.GetComponent<Drawer>().medicinesInDrawer.Clear(); //clear the medicines in shelf.

            compartments.Add(new Compartments(index, child.GetComponentsInChildren<Compartment>()));
            index++;
        }


        //place the items in the shelf
        for (int i = 0; i < XMLData.appData.mCabinets[XMLData.scenario.mCabinetID].mDrawers.Count && i < MAXDRAWERS; i++) //limits the drawers to 6
        {
            for (int j = 0; j < XMLData.appData.mDrawers[XMLData.appData.mCabinets[XMLData.scenario.mCabinetID].mDrawers[i]].mMedicines.Count && i < MAXCOMPORTMENTS; j++) //limits the medicines per drawer to 5
            {
                //medicines.Add(appData.mMedicines[appData.mDrawers[appData.mCabinets[scenarioToGetMedsFrom.mCabinetID].mDrawers[i]].mMedicines[j]]);
                //instantiate here that xmldata
                //Debug.Log("i : " + i + " j : " + j + " i count : " + XMLData.appData.mCabinets[XMLData.scenario.mCabinetID].mDrawers.Count + " j count :" + XMLData.appData.mDrawers[XMLData.appData.mCabinets[XMLData.scenario.mCabinetID].mDrawers[i]].mMedicines.Count  );
                //Debug.Log("Index : " + XMLData.appData.mDrawers[XMLData.appData.mCabinets[XMLData.scenario.mCabinetID].mDrawers[i]].mMedicines[j]);
                Medicine med = XMLData.appData.mMedicines[XMLData.appData.mDrawers[XMLData.appData.mCabinets[XMLData.scenario.mCabinetID].mDrawers[i]].mMedicines[j]];
                //Debug.Log("Medicine :" + med);
                GameObject medicineObject = Array.Find(medicinePrefabs, x => (x.medicineName.ToLower() == med.mName.ToLower()) && (x.medicinePackage == med.mPackage)).medicinePrefab; //looks if the medicine is in the medicinePrefabs array so it can know which prefabs belongs with it

                Vector3 position = compartments.Find(x => x._drawerID == i)._compartment[j].transform.position; 


                if (medicineObject != null) //if it found a match
                {
                    GameObject medicine = Instantiate(medicineObject, position, medicineObject.transform.rotation);
                    medicine.GetComponent<MedicineData>().medicine = med;
                    instantiatedMeds.Add(medicine);
                }
                else
                {
                    GameObject medicine = Instantiate(unknownMedicinePrefab, position, medicineObject.transform.rotation);
                    medicine.GetComponent<MedicineData>().medicine = med; //adds the medicine to the object
                    instantiatedMeds.Add(medicine);
                }

                //list which medicines are in which drawer 
                drawersGameObject.GetChild(i).GetComponent<Drawer>().medicinesInDrawer.Add(med.Name);
            }
        }

    }

	
}
