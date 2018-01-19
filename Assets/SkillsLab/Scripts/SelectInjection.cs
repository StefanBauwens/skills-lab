using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public enum InjectionOption
{
    IV = 0,
    IM,
    SC
}

public class SelectInjection : MonoBehaviour {
    public VRTK_ControllerEvents leftController;
    public VRTK_ControllerEvents rightController;
    public GameObject leftOptions;
    public GameObject rightOptions;

    public bool optionChosen;
    protected InjectionOption option; //this says which option has been chosen

	// Use this for initialization
	void Start () {
        optionChosen = false;
        //leftController.TouchpadPressed += new ControllerInteractionEventHandler(LeftTouchpadPressed);
        leftController.TouchpadReleased += new ControllerInteractionEventHandler(LeftTouchpadReleased);
        //rightController.TouchpadPressed += new ControllerInteractionEventHandler(RightTouchpadPressed);
        rightController.TouchpadReleased += new ControllerInteractionEventHandler(RightTouchpadReleased);
    }

    public void ResetOption()
    {
        optionChosen = false;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Left angle: " + leftController.GetTouchpadAxisAngle() + " right angle: " + rightController.GetTouchpadAxisAngle());

	}

    protected void EnableLeftOptions(bool enable)
    {
        leftOptions.SetActive(enable);
    }

    protected void EnableRightOptions(bool enable)
    {
        rightOptions.SetActive(enable);
    }

    //protected void LeftTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    //{

    //}

    protected void LeftTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (!optionChosen)
        {
            option = (InjectionOption)Mathf.FloorToInt(leftController.GetTouchpadAxisAngle() / (360 / 3));
            optionChosen = true;
            EnableLeftOptions(false);
            Debug.Log("Option = " + option.ToString());
        }

    }

    //protected void RightTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    //{

    //}

    protected void RightTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (!optionChosen)
        {
            option = (InjectionOption)Mathf.FloorToInt(rightController.GetTouchpadAxisAngle() / (360 / 3));
            optionChosen = true;
            EnableRightOptions(false);
            Debug.Log("Option = " + option.ToString());
        }
    }
}
