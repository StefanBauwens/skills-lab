using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System;
using System.IO;


public class CabinetDrawer
{
    [XmlAttribute]
    public int mID;
    public List<int> mMedicines;
    public List<int> mDeliveryTools;
    public bool mIsLocked;

    public CabinetDrawer()
    {
        mID = 0;
        mMedicines = new List<int>();
        mDeliveryTools = new List<int>(); //delivery tools don't belong in a vanas shelf
        mIsLocked = false; //plz. It should be always locked ok?
    }

}

