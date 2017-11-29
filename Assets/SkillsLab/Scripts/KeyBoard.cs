using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoard : MonoBehaviour {

    protected InputField inputF;
    public InputField firstName;
    public InputField lastName;
    public InputField medical;
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
        }
        else
        {
            results = search.SearchForName(firstName.text, lastName.text);
        }

        foreach (var result in results)
        {
            Debug.Log(result.ToString());
        }
    }

}
