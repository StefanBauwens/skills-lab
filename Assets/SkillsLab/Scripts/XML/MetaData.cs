using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System;
using System.IO;


public class MetaData
{
    public string mVersion;
    public DateTime mDateTime;
    public string mGenerator;

    public MetaData()
    {
        mVersion = "0.1";
        mDateTime = DateTime.Now;
        mGenerator = "KdG XML Generator v0.1";
    }
}

