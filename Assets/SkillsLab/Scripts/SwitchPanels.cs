using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanels : MonoBehaviour {
    public CanvasGroup panelSearch;
    public CanvasGroup panelResults;
    public CanvasGroup loginScreen;
    protected SearchResult currentResult;

    public Button logOutBtn;
    public Button retrieveButton;

	void Start () {
        Logout();
        currentResult = null;
        logOutBtn.onClick.AddListener(Logout);
        retrieveButton.onClick.AddListener(RetrieveButton);
	}

    public void Login()
    {
        BackButton();
        DisablePanel(loginScreen, true);
    }

    protected void Logout()
    {
        DisablePanel(panelResults, true);
        DisablePanel(panelSearch, true);
        DisablePanel(loginScreen, false);
    }

    public void ShowResult(SearchResult result)
    {
        if (result is /*Medical*/Medicine)
        {
            //retrieveButton.gameObject.SetActive(((/*Medical*/Medicine)result).Quantity > 0);
            //TEMP DISABLED ABOVE LINE BECAUSE XML IS CONFUSING
            retrieveButton.gameObject.SetActive(true);
        }
        else
        {
;            retrieveButton.gameObject.SetActive(false);
        }
        currentResult = result;
        panelResults.GetComponentInChildren<Text>().text = result.ToResult();
        DisablePanel(panelResults, false);
        DisablePanel(panelSearch, true);
    }

    public void BackButton()
    {
        DisablePanel(panelResults, true);
        DisablePanel(panelSearch, false);
    }

    protected void RetrieveButton()
    {
        //((/*Medical*/Medicine)currentResult).Quantity--; //SEE IF QUANTITY IS ABOUT HOW MANY PILLS IN A BOX, OR IF ITS ABOUT HOW MANY BOXES WITH PILLS. BIT CONFUSING WITH XML

        EventParam medicine = new EventParam();
        medicine.param1 = /*currentResult.Name;*/((Medicine)currentResult).Name;
        EventManagerParam.TriggerEvent(GameEvent.UNLOCK_DRAWER, medicine);
        //Debug.Log("medicine selected: " + currentResult.Name);
    }

    protected void DisablePanel(CanvasGroup panel, bool disablePanel)
    {
        panel.alpha = disablePanel?0:1;
        panel.interactable = !disablePanel;
        panel.blocksRaycasts = !disablePanel;
    }
}
