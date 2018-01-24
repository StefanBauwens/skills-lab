using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System;
using System.IO;

public class SearchResult {
    
    public virtual string ToResult()
    {
        //return string.Format("Name: {0}\nInfo:\n{1}", Name, Info);
        return string.Format("Name : blank");
    }
}
