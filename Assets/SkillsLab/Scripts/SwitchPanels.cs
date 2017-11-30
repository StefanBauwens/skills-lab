using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanels : MonoBehaviour {
    public CanvasGroup panelSearch;
    public CanvasGroup panelResults;

	// Use this for initialization
	void Start () {
        BackButton();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowResult(SearchResult result)
    {
        panelResults.GetComponentInChildren<Text>().text = result.ToResult();
        panelResults.alpha = 1;
        panelResults.interactable = true;
        panelResults.blocksRaycasts = true;
        panelSearch.alpha = 0;
        panelSearch.interactable = false;
        panelSearch.blocksRaycasts = false;

    }

    public void BackButton()
    {
        panelResults.alpha = 0;
        panelResults.interactable = false;
        panelResults.blocksRaycasts = false;
        panelSearch.alpha = 1;
        panelSearch.interactable = true;
        panelSearch.blocksRaycasts = true;

    }
}
