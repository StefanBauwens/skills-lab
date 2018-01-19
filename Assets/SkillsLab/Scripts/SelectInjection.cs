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

    protected bool isTouchingLeft;
    protected bool isTouchingRight;

    protected bool isReady;

    public bool optionChosen;
    protected InjectionOption option; //this says which option has been chosen

	// Use this for initialization
	void Start () {
        isReady = false;
        isTouchingLeft = false;
        isTouchingRight = false;
        optionChosen = false;
        leftController.TouchpadPressed += new ControllerInteractionEventHandler(LeftTouchpadPressed);
        leftController.TouchpadReleased += new ControllerInteractionEventHandler(LeftTouchpadReleased);
        rightController.TouchpadPressed += new ControllerInteractionEventHandler(RightTouchpadPressed);
        rightController.TouchpadReleased += new ControllerInteractionEventHandler(RightTouchpadReleased);
    }

    public void ResetOption()
    {
        optionChosen = false;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Left angle: " + leftController.GetTouchpadAxisAngle() + " right angle: " + rightController.GetTouchpadAxisAngle());
        if (isTouchingLeft)
        {
            //change hover materiar based on angle
            byte result = (byte)Mathf.FloorToInt(leftController.GetTouchpadAxisAngle() / (360 / 3));
            switch(result)
            {
                case 0:
                    leftOptions.GetComponent<Material>().mainTexture = ivSelectedTexture;
                    break;
                case 1:
                    leftOptions.GetComponent<Material>().mainTexture = imSelectedTexture; 
                    break;
                case 2:
                    leftOptions.GetComponent<Material>().mainTexture = scSelectedTexture;
                    break;
            }
        }
        if (isTouchingRight)
        {
            byte result = (byte)Mathf.FloorToInt(rightController.GetTouchpadAxisAngle() / (360 / 3));
            switch (result)
            {
                case 0:
                    rightOptions.GetComponent<Material>().mainTexture = ivSelectedTexture;
                    break;
                case 1:
                    rightOptions.GetComponent<Material>().mainTexture = imSelectedTexture;
                    break;
                case 2:
                    rightOptions.GetComponent<Material>().mainTexture = scSelectedTexture;
                    break;
            }
        }

    }

    protected void EnableLeftOptions(bool enable)
    {
        leftOptions.SetActive(enable);
    }

    protected void EnableRightOptions(bool enable)
    {
        rightOptions.SetActive(enable);
    }

    protected void LeftTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        isTouchingLeft |= !optionChosen;
    }

    protected void LeftTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (!optionChosen)
        {
            option = (InjectionOption)Mathf.FloorToInt(leftController.GetTouchpadAxisAngle() / (360 / 3));
            optionChosen = true;
            isTouchingLeft = false;
            EnableLeftOptions(false);
            Debug.Log("Option = " + option.ToString());
        }

    }

    protected void RightTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        isTouchingRight |= !optionChosen;
    }

    protected void RightTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (!optionChosen)
        {
            option = (InjectionOption)Mathf.FloorToInt(rightController.GetTouchpadAxisAngle() / (360 / 3));
            optionChosen = true;
            EnableRightOptions(false);
            isTouchingRight = false;
            Debug.Log("Option = " + option.ToString());
        }
    }
}
