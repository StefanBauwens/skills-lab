using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By Stefan
//This script requires ImportXML to run first

public static class XMLData {
    public static MedicalAppData appData;
    public static Scenario scenario;

    //How to get data:
    //appData.mPatients[scenario.mPatientID]; 
    //appData.mCabinets[scenario.mCabinetID];
    //appData.mMedicines[scenario.mMedicineID];
    //appData.mMethods[scenario.mDeliveryMethod];

    public static List<Medicine> GetMedicinesFromScenario(Scenario scenarioToGetMedsFrom) //returns a list of all the medicines found in the cabinet of the specific scenario
    {
        List<Medicine> medicines = new List<Medicine>();
        try
        {
            for (int i = 0; i < appData.mCabinets[scenarioToGetMedsFrom.mCabinetID].mDrawers.Count; i++)
            {
                for (int j = 0; j < appData.mDrawers[appData.mCabinets[scenarioToGetMedsFrom.mCabinetID].mDrawers[i]].mMedicines.Count; j++)
                {
                    medicines.Add(appData.mMedicines[appData.mDrawers[appData.mCabinets[scenarioToGetMedsFrom.mCabinetID].mDrawers[i]].mMedicines[j]]);
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.Log("Error! There's likely an index error in your XML. Exception given :" + ex); //read.xml has index mistakes :rolleyes:
        }

        return medicines;
    }
}
