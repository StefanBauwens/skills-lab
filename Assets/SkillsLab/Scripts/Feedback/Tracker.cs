using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SyringeData{
    public float syringeValue;
    public NeedleOption needleToUse;
    public float amountToPull;

    public SyringeData(float value, NeedleOption needle, float amount)
    {
        syringeValue = value;
        needleToUse = needle;
        amountToPull = amount;
    }
}

//This script logs the interactions to see if player is making any mistakes.
//The values are changed from other scripts
public static class Tracker {

    const string SYRINGE = "syringe";
    public static bool usingSyringe = false;
    
    //check if correct patient
    public static Patient patient = new Patient(); //the target patient
    public static int wrongPatient = 0; //amount of times interacted (as in giving medicine or something) with wrong patient
    public static bool interactedWithCorrectPatient = false; //if interacted with correct patient (at least once) (interaction = needle, cup of water, pill, iv hand)
    public static bool checkPatient = false; //if you checked your patient on vanas or tablet (should work with scanning or with searching!)

    //check if correct medicine & Vanas
    public static Medicine medicine = new Medicine(); //the target medicine to use
    public static int wrongMedicines = 0; //amount of times wrong medicine retrieved from Vanas (adds up when clicking on retrieve button) 
    public static bool correctMedicineRetrieved = false; //correct medicine retrieved from Vanas
    public static bool correctMedicineGiven = false; //for syringe as well as for other meds (= medicine in syringe, pills, cup of water with medicine)
    public static int quantityApplied = 0; //how many times has the medicine been given (to correct patient) //only for non syringe meds (= pills, cup of water with medicine)

    //needle:
    public static float amountOfLiquidApplied = 0f; //check if this value is equal to the deliverytool_amountToTake (only checks if it's the correct medicine) //based on this make a bool to check if correct amount was given
    public static SyringeData syringeData; //contains info about which syringe and which needle to use
    public static bool correctNeedle = false; //used SC, IV, IM (or Transfer) needle
    public static bool correctInjectionMethod = false; //used needle in a SC, IV or IM manner (chosen with controller)
    public static bool correctPlaceOnBody = false; //e.g. SC can only be used on specific place
    public static bool correctSyringe = false;

    //Not using deliverymethod system since it's problematic
    //check if correct deliverymethod -->syringe#syringeCapacity#amountToTake
    //public static List<DeliveryTool> deliveryTools = new List<DeliveryTool>(); //deliverytools are syringe, cup of water, needlecontainer, IV_hand
    //add when interacted with (e.G. snap on body, snap needle on container)

    public static void ResetTracking()
    {
        //REMEMBER ONLY to call this after XMLDATA has been updated with new values
        patient = XMLData.appData.mPatients[XMLData.scenario.mPatientID];
        medicine = XMLData.appData.mMedicines[XMLData.scenario.mMedicineID];
        if (medicine.mName.Contains("#"))
        {
            medicine = medicine.CleanUpName();
        }

        List<string> deliveryMethodFromXML = new List<string>(); //gets a string list of the deliverymethod(list of tools) from active scenario
        foreach (int index in XMLData.appData.mMethods[XMLData.scenario.mDeliveryMethod].mTools)
        {
            try
            {
                deliveryMethodFromXML.Add(XMLData.appData.mTools[index].mName.ToLower());
            }
            catch (System.Exception e)
            {
                Debug.Log("Error trying to read in DeliveryTools from XML. Check XML. Error:" + e);
            }

        }

        usingSyringe = false;
        List<SyringeData> syringesToUse = new List<SyringeData>(); //For advanced use later. Right now only using one syringe
        foreach (var item in deliveryMethodFromXML)
        {
            if (item.Contains(SYRINGE)) //if this is the case it should have following structure : syringe#50#IV#2.5(amount to pull)
            {
                usingSyringe = true;
                string[] values = item.Split('#');
                NeedleOption needleOption = NeedleOption.IV;

                switch(values[2].ToLower())
                {
                    case "iv":
                        needleOption = NeedleOption.IV;
                        break;
                    case "im":
                        needleOption = NeedleOption.IM;
                        break;
                    case "sc":
                        needleOption = NeedleOption.SC;
                        break;
                    case "transfer":
                        needleOption = NeedleOption.Transfer;
                        break;
                    default:
                        Debug.Log("There might be a problem with your XML. Invalid needle type.");
                        break;        
                }
                syringesToUse.Add(new SyringeData(float.Parse(values[1]), needleOption, float.Parse(values[3])));
            }
        }
        if (usingSyringe)
        {
            syringeData = syringesToUse[0]; //only using first one for now. 
        }


        wrongPatient = 0;
        interactedWithCorrectPatient = false;
        checkPatient = false;
        wrongMedicines = 0;
        correctMedicineRetrieved = false;
        quantityApplied = 0;
        amountOfLiquidApplied = 0f;
        correctNeedle = false;
        correctInjectionMethod = false;
        correctPlaceOnBody = false;
        correctSyringe = false;
    }
}
