using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System;
using System.IO;

/*
[Serializeble]
public enum DeliveryMethod
{
    PO_Oplosbaar = 1,
    PO_Tablet,
    SC,
    IM,
    IV
}*/

public class Scenario  {
    [XmlAttribute]
    public int mID;
    public string mName;
    public int mPatientID;
    public int mCabinetID;
    public int mMedicineID;
    public int mDeliveryMethod;

    public Scenario()
    {
        mID = 0;
        mName = ""; //Divide name and description with #
        mPatientID = 0;
        mCabinetID = 0;
        mMedicineID = 0;
        mDeliveryMethod = 0; //ID I assume?
    }

   

	
}
