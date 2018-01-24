using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    public SearchVanas search;
    protected SearchResult[] results;

    protected bool ignoredValueChange;


	// Use this for initi1alization
	public void Start () {
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
        inputF.caretPosition = 0;
        if (character == "←")
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
        if(inputF.text.Length >= 2)
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
            resultsPatients.Hide();
            GoButtonPatient.gameObject.SetActive(false);
            resultsMedical.interactable = true;
            GoButtonMedical.gameObject.SetActive(true);

            results = search.SearchForMedical(medical.text);
            resultsMedical.options.Clear();
            foreach (var item in results)
            {
                resultsMedical.options.Add(new Dropdown.OptionData(((Medicine)item).Name));
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
                //resultsMedical.Hide();
                resultsMedical.GetComponentInChildren<Text>().text = "Results";
                resultsMedical.interactable = true;
                //resultsMedical.Show();
                StartCoroutine(RefreshDropdown(resultsMedical));
            }
        }
        else
        {
            resultsMedical.Hide();
            resultsPatients.interactable = true;
            GoButtonPatient.gameObject.SetActive(true);
            resultsMedical.interactable = false;
            GoButtonMedical.gameObject.SetActive(false);

            results = search.SearchForName(firstName.text, lastName.text);
            resultsPatients.options.Clear();
            foreach (var item in results)
            {
                //resultsPatients.options.Add(new Dropdown.OptionData(item.Name + " " + ((Patient)item).FirstName));
                resultsPatients.options.Add(new Dropdown.OptionData(((Patient)item).Name));
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
                //resultsPatients.Hide();
                resultsPatients.GetComponentInChildren<Text>().text = "Results";
                resultsPatients.interactable = true;
                //resultsPatients.Show();
                StartCoroutine(RefreshDropdown(resultsPatients));

            }

        }
        DisableBlocker();

        foreach (var result in results)
        {
            Debug.Log(result.ToString());
        }
    }

    protected void DisableBlocker()
    {
        Transform blocker = null;
        blocker = this.gameObject.transform.parent.parent.Find("Blocker");
        if (blocker != null)
        {
            Destroy(blocker.gameObject);
        }
    }

    protected IEnumerator RefreshDropdown(Dropdown dropdown)
    {
        dropdown.Hide();
        yield return new WaitForSeconds(0.2f);
        dropdown.Show();
        DisableBlocker();
    }

}
