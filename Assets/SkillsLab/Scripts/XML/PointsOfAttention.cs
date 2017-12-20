using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System;
using System.IO;


public class PointsOfAttention  {
    [XmlAttribute]
    public int mID;
    public string mText;
   

	public PointsOfAttention()
    {
        mText = "";
        mID = 0;
    }
}
