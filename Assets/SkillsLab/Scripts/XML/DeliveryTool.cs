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
    IV,
    Glas_Water,
    Inhalatie_Puffer,
    Inhalatie_Aerosol,
    Spuit
}*/


public class DeliveryTool  {

    [XmlAttribute]
    public int mID;
    public string mName;

    public DeliveryTool()
    {
        mID = 0;
        mName = "";
    }
}
