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

    public Texture ivSelectedTexture;
    public Texture imSelectedTexture;
    public Texture scSelectedTexture;

    protected PullSyringe subscribedSyringe;

    protected bool isTouchingLeft;
    protected bool isTouchingRight;

    public bool leftHand;

    public bool optionChosen;
    protected InjectionOption option; //this says which option has been chosen

	// Use this for initialization
	void Start () {
        leftHand = false;
        isTouchingLeft = false;
        isTouchingRight = false;
        optionChosen = true;
        leftController.TouchpadPressed += new ControllerInteractionEventHandler(LeftTouchpadPressed);
        leftController.TouchpadReleased += new ControllerInteractionEventHandler(LeftTouchpadReleased);
        rightController.TouchpadPressed += new ControllerInteractionEventHandler(RightTouchpadPressed);
        rightController.TouchpadReleased += new ControllerInteractionEventHandler(RightTouchpadReleased);
    }

    public void ResetOption()
    {
        optionChosen = false;
    }

    public void Subscribe(PullSyringe syringe)
    {
        subscribedSyringe = syringe;
    }
	
	// Update is called once per frame
	void Update () {
        if (isTouchingLeft)
        {
            //change hover materiar based on angle
            byte result = (byte)Mathf.FloorToInt(leftController.GetTouchpadAxisAngle() / (360 / 3));
            switch(result)
            {
                case 0:
                    leftOptions.GetComponent<MeshRenderer>().material.mainTexture = ivSelectedTexture;
                    break;
                case 1:
                    leftOptions.GetComponent<MeshRenderer>().material.mainTexture = imSelectedTexture; 
                    break;
                case 2:
                    leftOptions.GetComponent<MeshRenderer>().material.mainTexture = scSelectedTexture;
                    break;
            }
        }
        if (isTouchingRight)
        {
            byte result = (byte)Mathf.FloorToInt(rightController.GetTouchpadAxisAngle() / (360 / 3));
            switch (result)
            {
                case 0:
                    rightOptions.GetComponent<MeshRenderer>().material.mainTexture = ivSelectedTexture;
                    break;
                case 1:
                    rightOptions.GetComponent<MeshRenderer>().material.mainTexture = imSelectedTexture;
                    break;
                case 2:
                    rightOptions.GetComponent<MeshRenderer>().material.mainTexture = scSelectedTexture;
                    break;
            }
        }

    }

    public void EnableLeftOptions(bool enable)
    {
        leftOptions.SetActive(enable);
    }

    public void EnableRightOptions(bool enable)
    {
        rightOptions.SetActive(enable);
    }

    protected void LeftTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (leftHand)
        {
            isTouchingLeft |= !optionChosen;
        }
    }

    protected void LeftTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (!optionChosen && leftHand)
        {
            option = (InjectionOption)Mathf.FloorToInt(leftController.GetTouchpadAxisAngle() / (360 / 3));
            optionChosen = true;
            isTouchingLeft = false;
            EnableLeftOptions(false);
            subscribedSyringe.StopChoosing();
            Debug.Log("Option = " + option.ToString());
        }

    }

    protected void RightTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (!leftHand)
        {
            isTouchingRight |= !optionChosen;
        }
    }

    protected void RightTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (!optionChosen && !leftHand)
        {
            option = (InjectionOption)Mathf.FloorToInt(rightController.GetTouchpadAxisAngle() / (360 / 3));
            optionChosen = true;
            EnableRightOptions(false);
            isTouchingRight = false;
            subscribedSyringe.StopChoosing();
            Debug.Log("Option = " + option.ToString());
        }
    }
}
