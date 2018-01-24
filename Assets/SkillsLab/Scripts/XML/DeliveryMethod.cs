using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System;
using System.IO;


public class DeliveryMethod  {

    [XmlAttribute]
    public int mID;
    public string mName;
    public List<int> mTools;

    public DeliveryMethod()
    {
        mID = 0;
        mName = "";
        mTools = new List<int>();
    }
}
