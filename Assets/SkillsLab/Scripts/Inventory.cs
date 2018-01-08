using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Inventory : MonoBehaviour {

    public GameObject leftControllerSteam;
    public GameObject rightControllerSteam;

    protected Transform leftModel;
    protected Transform rightModel;

    protected Transform leftController;
    protected Transform rightController;
    protected VRTK_ControllerEvents leftEvents;
    protected VRTK_ControllerEvents rightEvents;

    protected Transform tray;
    protected Vector3 startPosition;
    protected Vector3 startEulerAngles;

    protected bool isLeft;
    protected bool isEnabled;

    const string VRTKSCRIPT = "vrtk_scripts";
    const string LCONTR = "LeftController";
    const string RCONTR = "RightController";
    const string TRAY = "TrayInventory";
    const string CNTRLMODEL = "Model";

    // Use this for initialization
    void Start () {
        GameObject vrtkScripts = GameObject.FindGameObjectWithTag(VRTKSCRIPT);
        leftController = vrtkScripts.transform.Find(LCONTR);
        rightController = vrtkScripts.transform.Find(RCONTR);
        leftEvents = leftController.GetComponent<VRTK_ControllerEvents>();
        rightEvents = rightController.GetComponent<VRTK_ControllerEvents>();
        tray = leftControllerSteam.transform.Find(TRAY);

        leftModel = leftControllerSteam.transform.Find(CNTRLMODEL);
        rightModel = rightControllerSteam.transform.Find(CNTRLMODEL);

        leftEvents.ButtonTwoPressed += new ControllerInteractionEventHandler(LeftBtnTwo);
        rightEvents.ButtonTwoPressed += new ControllerInteractionEventHandler(RightBtnTwo);

        isLeft = true;
        isEnabled = false;

        startPosition = tray.localPosition;
        startEulerAngles = tray.localEulerAngles;
    }

    protected void LeftBtnTwo(object sender, ControllerInteractionEventArgs e)
    {
        if (!isEnabled)
        {
            isEnabled = true;
            isLeft = true;
        }
        else if (isEnabled && isLeft)
        {
            isEnabled = false;
        }
        else if (isEnabled && !isLeft) //if inventory is enabled and showing on the right hand switch to left hand
        {
            isLeft = true;
        }
        ShowController();
        ShowTray();
    }

    protected void RightBtnTwo(object sender, ControllerInteractionEventArgs e)
    {
        if (!isEnabled) //if tray isn't enabled yet
        {
            isEnabled = true;
            isLeft = false;
        }
        else if (isEnabled && !isLeft) //if tray is enabled and you're holding it with your right hand
        {
            isEnabled = false;
        }
        else if (isEnabled && isLeft) //if inventory is enabled and showing on the left hand switch to right hand
        {
            isLeft = false;
        }
        
        ShowController();
        ShowTray();
    }

    protected void ShowTray()
    {
        tray.SetParent(isLeft ? leftControllerSteam.transform : rightControllerSteam.transform);
        tray.localPosition = startPosition;
        tray.localEulerAngles = startEulerAngles;
        tray.gameObject.SetActive(isEnabled);    
    }

    protected void ShowController()
    {
        if (!isLeft)
        {
            if (isEnabled)
            {
                ToggleController(rightController, false);
            }
            else
            {
                ToggleController(rightController, true);
            }
            ToggleController(leftController, true);
        }
        else if (isLeft)
        {
            if (isEnabled)
            {
                ToggleController(leftController, false);
            }
            else
            {
                ToggleController(leftController, true);

            }
            ToggleController(rightController, true);
        }
    }

    protected void ToggleController(Transform controller, bool enabled)
    {
        if (controller == rightController)
        {
            rightModel.gameObject.SetActive(enabled);
        }
        else
        {
            leftModel.gameObject.SetActive(enabled);
        }
        controller.gameObject.SetActive(enabled);
        controller.GetComponent<VRTK_InteractGrab>().enabled = enabled;
        controller.GetComponent<VRTK_Pointer>().enabled = enabled;
    }
}
