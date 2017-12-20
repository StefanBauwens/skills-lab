using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportXML : MonoBehaviour {
    MedicalAppData appData = new MedicalAppData();

	// Use this for initialization
	void Start () {
        bool success = MedicalAppData.ReadFromFile("read.xml", out appData);
        Debug.Log("Reading file succeeded? " + success );
    }
}
