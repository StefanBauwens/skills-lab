using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoard : MonoBehaviour {
    public InputField inputF;

	// Use this for initi1alization
	void Start () {
		
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
        //do something on vanas kast
    }

}
