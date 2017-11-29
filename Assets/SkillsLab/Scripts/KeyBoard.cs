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

    protected SearchVanas search;

	// Use this for initi1alization
	void Start () {
        inputF = firstName;
        search = new SearchVanas();
        search.Start();
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

    public void TypeKey(string character)
    {
        if (character == "⌫")
        {
            BackSpace();
            return;
        }
        else if (character == "↩")
        {
            Submit();
            return;
        }
        inputF.text += character;
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
        SearchResult[] results;
        if (inputF == medical)
        {
            results = search.SearchForMedical(medical.text);
            resultsMedical.options.Clear();
            foreach (var item in results)
            {
                resultsMedical.options.Add(new Dropdown.OptionData(item.Name));
            }
            if (results.Length == 0)
            {
                resultsMedical.GetComponentInChildren<Text>().text = "No results";
                resultsMedical.interactable = false;
            }
            else
            {
                resultsMedical.GetComponentInChildren<Text>().text = "Results";
                resultsMedical.interactable = true;
            }
        }
        else
        {
            results = search.SearchForName(firstName.text, lastName.text);
            resultsPatients.options.Clear();
            foreach (var item in results)
            {
                resultsPatients.options.Add(new Dropdown.OptionData(item.Name + " " + ((Patient)item).FirstName));
            }
            if (results.Length == 0)
            {
                resultsPatients.GetComponentInChildren<Text>().text = "No results";
                resultsPatients.interactable = false;
            }
            else
            {
                resultsPatients.GetComponentInChildren<Text>().text = "Results";
                resultsPatients.interactable = true;
            }
        }

        foreach (var result in results)
        {
            Debug.Log(result.ToString());
        }
    }

}
