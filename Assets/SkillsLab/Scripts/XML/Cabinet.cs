using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System;
using System.IO;

public class Cabinet  {

    [XmlAttribute]
    public int mID;
    public List<int> mDrawers;//list van ids van drawers 

    public Cabinet()
    { 
		mID = 0;
        mDrawers = new List<int>();
	}
}
