using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Choose through UI (another) scenario. By default ImportXML.cs will set the first scenario found in the list

public class ScenarioPicker : MonoBehaviour {
    public Dropdown dropdown;
    public LoadPatientData loadPatientData;  //FILL ALL THESE IN IN INSPECTORs
    public LoadVanas loadVanas;
    public LoadGray loadGray;
    public KeyBoard[] keyboards;

    public CanvasGroup loadPanel;
    public CanvasGroup descriptionPanel;
    public CanvasGroup reportPanel;

    protected Text descriptionPanelText;

    protected List<Dropdown.OptionData> scenarioOptions = new List<Dropdown.OptionData>();

	// Use this for initialization
	void Start () {
        descriptionPanelText = descriptionPanel.GetComponentInChildren<Text>();

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

        ShowDescription(); //shows the description of the default loaded scenario.
    }
	
	void Update () {
		
	}

    protected int PatientToNumber(PatientType type) //gives the number of the room the patient is in.
    {
        int result = 0;
        switch(type)
        {
            case PatientType.adult:
                result = 306;
                break;
            case PatientType.child:
                result = 305;
                break;
            case PatientType.pregnant:
                result = 304;
                break;
            case PatientType.senior:
                result = 307;
                break;
            default:
                result = -1;
                break;
        }
        return result;
    }

    public void ButtonPress() //loadscene
    {
        LoadScenario(dropdown.value);
        ShowDescription();
    }

    protected void ShowDescription()
    {
        if (XMLData.scenario.mName.Contains("#"))
        {
            descriptionPanelText.text = XMLData.scenario.mName.Split('#')[0] + "\n\n" + XMLData.scenario.mName.Split('#')[1];
        }
        else
        {
            descriptionPanelText.text = XMLData.scenario.mName + "\n\n" + "No description available"; 
        }
        Patient patient = XMLData.appData.mPatients[XMLData.scenario.mPatientID];
        descriptionPanelText.text += ("\nThe patient " + patient.Name + " can be found in room " + PatientToNumber(patient.mType) + "."); 

        EnablePanel(loadPanel, false);
        EnablePanel(descriptionPanel, true);
    }

    public void ButtonFinish()
    {
        //fill in reportPanel
        string textValue = "<b>REPORT</b>\n\nInteracted with correct patient: " + Answer(Tracker.interactedWithCorrectPatient);
        textValue += ("Amount of times interacted with incorrect patient: " + Answer(Tracker.wrongPatient));
        textValue += ("Checked patient on tablet or Vanas: " + Answer(Tracker.checkPatient));
        textValue += ("Retrieved correct medicine from Vanas: " + Answer(Tracker.correctMedicineRetrieved));
        textValue += ("Amount of times retrieved incorrect medicine" + Answer(Tracker.wrongMedicines));
        textValue += ("Correct medicine given to patient: " + Answer(Tracker.correctMedicineGiven));
        if (Tracker.usingSyringe)
        {
            textValue += ("Correct amount of medicine injected: " + Answer(Tracker.amountOfLiquidApplied == Tracker.syringeData.amountToPull));
            textValue += ("Correct syringe used: " + Answer(Tracker.correctSyringe));
            textValue += ("Correct needle used: " + Answer(Tracker.correctNeedle));
            textValue += ("Correct injection method chosen: " + Answer(Tracker.correctInjectionMethod));
            textValue += ("Injected on correct place on body: " + Answer(Tracker.correctPlaceOnBody));
        }
        else
        {
            textValue += ("Amount of times given medicine: " + Answer(Tracker.quantityApplied, 1));
        }

        reportPanel.GetComponentInChildren<Text>().text = textValue;

        EnablePanel(loadPanel, false);
        EnablePanel(reportPanel, true);
    }

    protected string Answer(bool boolValue)
    {
        if (boolValue)
        {
            return "<b><color=#00ff00ff>Yes</color></b>\n";
        }
        else
        {
            return "<b>< color=#ff0000ff>No</color></b>\n";
        }
    }

    protected string Answer(int value)
    {
        if (value == 0)
        {
            return "<b><color=#00ff00ff>0</color></b>\n";
        }
        else
        {
            return ("<b>< color=#ff0000ff>" + value + "</color></b>\n");
        }
    }

    protected string Answer(int value, int correctValue)
    {
        if (value == correctValue)
        {
            return ("<b><color=#00ff00ff>" + correctValue + "</color></b>\n");
        }
        else
        {
            return ("<b>< color=#ff0000ff>" + value + "</color></b>\n");
        }
    }


    public void ButtonBack()
    {
        EnablePanel(loadPanel, true);
        EnablePanel(descriptionPanel, false);
        EnablePanel(reportPanel, false);
    }

    protected void EnablePanel(CanvasGroup panel, bool enable)
    {
        panel.alpha = enable ? 1 : 0;
        panel.interactable = enable;
        panel.blocksRaycasts = enable;
    }

    protected void LoadScenario(int index)
    {
        try
        {
            XMLData.scenario = XMLData.appData.mScenarios[index];

            //load everything again:
            loadPatientData.Start(); //loads patient
            loadVanas.Start();
            loadGray.Start();
            Tracker.patient = XMLData.appData.mPatients[XMLData.scenario.mPatientID];
           
            foreach (var keyboard in keyboards)
            {
                keyboard.Start(); //reloads medicines in vanas
            }
            Debug.Log("Loaded new scenario!");
                           
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error loading scenario: " + ex);
        }
    }
}
