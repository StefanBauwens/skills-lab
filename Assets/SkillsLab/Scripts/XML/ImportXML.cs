using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//BY Stefan

public class ImportXML : MonoBehaviour {
    protected MedicalAppData appData = new MedicalAppData();
    //protected string scenarioToLoad;
    //protected Scenario scenarioToLoad; 
        
	void Awake () { //use awake so by Start in other classes XMLData class will be already filled in
        bool success = MedicalAppData.ReadFromFile("read.xml", out appData); //try reading in the xml
        Debug.Log("Reading file succeeded? " + success );
        XMLData.appData = this.appData; //copy the xml data to the static XMLData class so it's accesible from every script. You're welcome.
        if (XMLData.appData.mScenarios[0] != null)
        {
            XMLData.scenario = XMLData.appData.mScenarios[0]; //get first scenario and set it as default
        }
        else
        {
            Debug.Log("Problem loading scenario. Are you sure there is a given scenario in your XML?");
        }

        //StartScenario(scenarioToLoad);
    }

    /*public void StartScenario(scenario scenarioName)
    {
        Scenario scenario = appData.mScenarios.Find(x => x.mName == scenarioName);
        if (scenario == null)
        {
            Debug.Log("Could not load scenario: " + scenarioName);
        }
        else
        {
            XMLData.scenario = scenario;
        }
    }*/
}
