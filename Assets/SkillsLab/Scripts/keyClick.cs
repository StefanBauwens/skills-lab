using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyClick : MonoBehaviour {

    protected Button thisBtn;
    public KeyBoard keyboardScript;
    protected string thisValue;

	// Use this for initialization
	void Start () {
        thisBtn = this.GetComponent<Button>();
        thisBtn.onClick.AddListener(PressButton);
        thisValue = thisBtn.GetComponentInChildren<Text>().text;
	}
	
    void PressButton()
    {
        keyboardScript.TypeKey(thisValue);
    }
}
