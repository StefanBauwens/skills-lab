using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoard : MonoBehaviour {

    protected InputField inputF;
    public InputField firstName;
    public InputField lastName;
    public InputField medical;
    public Dropdown resultsPatients;
    public Dropdown resultsMedical;
    public SwitchPanels switcher;
    public Button GoButtonPatient;
    public Button GoButtonMedical;

    protected SearchVanas search;
    protected SearchResult[] results;

    protected bool ignoredValueChange;


	// Use this for initi1alization
	void Start () {
        ignoredValueChange = false;
        inputF = firstName;
        search = new SearchVanas();
        search.Start();
        resultsMedical.Hide();
        resultsPatients.Hide();
        GoButtonMedical.gameObject.SetActive(false);
        GoButtonPatient.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (firstName.isFocused)
        {
            inputF = firstName;
        }
        else if (lastName.isFocused)
        {
            inputF = lastName;
        }
        else if (medical.isFocused)
        {
            inputF = medical;
        }
    }

    public void SelectResultMedical()//is called when player selects a result
    {
        if (ignoredValueChange || results.Length == resultsMedical.value)
        {
            return;
        }
        switcher.ShowResult(results[resultsMedical.value]);
    }

    public void SelectResultPatient()//is called when player selects a result
    {
        if (ignoredValueChange || results.Length == resultsPatients.value)
        {
            return;
        }
        switcher.ShowResult(results[resultsPatients.value]);
    }

    public void TypeKey(string character)
    {
        if (character == "←")
        {
            BackSpace();
            inputF.caretPosition--;
            return;
        }
        else if (character == "↩")
        {
            Submit();
            return;
        }
        else
        {
            inputF.caretPosition++;
        }
        inputF.text += character;
        if(inputF.text.Length >= 3)
        {
            Submit();
        }
    }

    public void BackSpace()
    {
        if (inputF.text.Length > 0)
        {
            inputF.text = inputF.text.Substring(0, inputF.text.Length - 1);
        }
    }

    public void Submit()
    {
        if (inputF == medical)
        {
            resultsPatients.interactable = false;
            GoButtonPatient.gameObject.SetActive(false);
            resultsMedical.interactable = true;
            GoButtonMedical.gameObject.SetActive(true);

            results = search.SearchForMedical(medical.text);
            resultsMedical.options.Clear();
            foreach (var item in results)
            {
                resultsMedical.options.Add(new Dropdown.OptionData(item.Name));
            }
            resultsMedical.options.Add(new Dropdown.OptionData("None"));
            ignoredValueChange = true;
            resultsMedical.value = resultsMedical.options.Count - 1;
            ignoredValueChange = false;

            if (results.Length == 0)
            {
                resultsMedical.GetComponentInChildren<Text>().text = "No results";
                resultsMedical.interactable = false;
                resultsMedical.Hide();
            }
            else
            {
                resultsMedical.GetComponentInChildren<Text>().text = "Results";
                resultsMedical.interactable = true;
                resultsMedical.Show();
            }
        }
        else
        {
            resultsPatients.interactable = true;
            //GoButtonPatient.interactable = true;
            GoButtonPatient.gameObject.SetActive(true);
            resultsMedical.interactable = false;
            //GoButtonMedical.interactable = false;
            GoButtonMedical.gameObject.SetActive(false);

            results = search.SearchForName(firstName.text, lastName.text);
            resultsPatients.options.Clear();
            foreach (var item in results)
            {
                resultsPatients.options.Add(new Dropdown.OptionData(item.Name + " " + ((Patient)item).FirstName));
            }
            resultsPatients.options.Add(new Dropdown.OptionData("None"));
            ignoredValueChange = true;
            resultsPatients.value = resultsMedical.options.Count-1;
            ignoredValueChange = false;
            if (results.Length == 0)
            {
                resultsPatients.GetComponentInChildren<Text>().text = "No results";
                resultsPatients.interactable = false;
                resultsPatients.Hide();
            }
            else
            {
                resultsPatients.GetComponentInChildren<Text>().text = "Results";
                resultsPatients.interactable = true;
                resultsPatients.Show();
            }
        }

        foreach (var result in results)
        {
            Debug.Log(result.ToString());
        }
    }

}
