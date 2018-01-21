using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Choose through UI (another) scenario. By default ImportXML.cs will set the first scenario found in the list

public class ScenarioPicker : MonoBehaviour {
    public Dropdown dropdown;
    public LoadPatientData loadPatientData;  //FILL ALL THESE IN IN INSPECTORs
    public LoadVanas loadVanas;
    public SearchVanas searchVanas;

    public CanvasGroup loadPanel;
    public CanvasGroup descriptionPanel;

    protected List<Dropdown.OptionData> scenarioOptions = new List<Dropdown.OptionData>();

	// Use this for initialization
	void Start () {
        for (int i = 0; i < XMLData.appData.mScenarios.Count; i++)
        {
            string nameDescription = XMLData.appData.mScenarios[i].mName;
            if (nameDescription.Contains("#")) //if it's a name and has a description
            {
                nameDescription = nameDescription.Split('#')[0];
            }
            scenarioOptions.Add(new Dropdown.OptionData(nameDescription));
        }
        dropdown.options = scenarioOptions;
    }
	
	void Update () {
		
	}

    public void ButtonPress() //loadscene
    {
        LoadScenario(dropdown.value);
        if (XMLData.scenario.mName.Contains("#"))
        {
            descriptionPanel.GetComponentInChildren<Text>().text = XMLData.scenario.mName.Split('#')[0] + "\n\n" + XMLData.scenario.mName.Split('#')[1];
        }
        else
        {
            descriptionPanel.GetComponentInChildren<Text>().text = XMLData.scenario.mName + "\n\n" + "No description available";
        }
        loadPanel.alpha = 0;
        loadPanel.interactable = false;
        loadPanel.blocksRaycasts = false;

        descriptionPanel.alpha = 1;
        descriptionPanel.interactable = true;
        descriptionPanel.blocksRaycasts = true;
    }

    public void ButtonBack()
    {
        loadPanel.alpha = 1;
        loadPanel.interactable = true;
        loadPanel.blocksRaycasts = true;

        descriptionPanel.alpha = 0;
        descriptionPanel.interactable = false;
        descriptionPanel.blocksRaycasts = false;
    }

    protected void LoadScenario(int index)
    {
        try
        {
            XMLData.scenario = XMLData.appData.mScenarios[index];

            //load everything again:
            loadPatientData.Start(); //loads patient
            loadVanas.Start();
            searchVanas.Start(); //reloads medicines in vanas
            Debug.Log("Loaded new scenario!");
                           
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error loading scenario: " + ex);
        }
    }
}
