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
        if (result is Medical)
        {
            retrieveButton.gameObject.SetActive(((Medical)result).Quantity > 0);
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
        ((Medical)currentResult).Quantity--;

        EventParam medicine = new EventParam();
        medicine.param1 = currentResult.Name;
        EventManagerParam.TriggerEvent(GameEvent.UNLOCK_DRAWER, medicine);
    }

    protected void DisablePanel(CanvasGroup panel, bool disablePanel)
    {
        panel.alpha = disablePanel?0:1;
        panel.interactable = !disablePanel;
        panel.blocksRaycasts = !disablePanel;
    }
}
