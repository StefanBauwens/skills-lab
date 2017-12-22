using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By Stefan
//UP to 5 items in one drawer of a blue vanas shelf (because 5 compartments)
//This script is placed on the BlueVanas

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
    const int MAXDRAWERS = 6;
    const int MAXCOMPORTMENTS = 5;
    const string DRAWERS = "Drawers";
    protected List<Compartments> compartments;

	// Use this for initialization
	void Start () {
        Transform drawersGameObject = this.transform.Find(DRAWERS);
        int index = 0;

        foreach (Transform child in drawersGameObject) //makes a list of compartments
        {
            compartments.Add(new Compartments(index, child.GetComponentsInChildren<Compartment>()));
            index++;
        }

        //place the items in the shelf
        for (int i = 0; i < XMLData.appData.mCabinets[XMLData.scenario.mCabinetID].mDrawers.Count || i < MAXDRAWERS; i++) //limits the drawers to 6
        {
            for (int j = 0; j < XMLData.appData.mDrawers[XMLData.appData.mCabinets[XMLData.scenario.mCabinetID].mDrawers[i]].mMedicines.Count || i < MAXCOMPORTMENTS; j++) //limits the medicines per drawer to 5
            {
                //medicines.Add(appData.mMedicines[appData.mDrawers[appData.mCabinets[scenarioToGetMedsFrom.mCabinetID].mDrawers[i]].mMedicines[j]]);
                //MAKE A DICTIONARY OR STRUCT LIST SO WE KNOW WHAT MEDICINE HAS WHAT GAMEOBJECT
                //instantiate here that xmldata
            }
        }
        XMLData.appData.mCabinets[XMLData.scenario.mCabinetID].mDrawers

    }

	
}
