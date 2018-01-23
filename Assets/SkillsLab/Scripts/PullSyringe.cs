using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;
using System.Globalization;

public class PullSyringe : MonoBehaviour {

    private VRTK_InteractableObject interactScript;
    protected Transform snapDrop;
    protected Transform insideSyringe;
    protected Transform fillWater;
    public float speed = 0.1f;
    public float maxMove = 0.1f;
    public float syringeValue = 10; //10ml
    protected Vector3 beginPosition;
    protected Vector3 beginPositionWater;
    protected bool isPulling;
    protected bool isPushing;
    protected bool grabbedByLeftHand;
    protected Text lcdText;
    protected Transform lcdCanvas;
    protected Transform leftController;
    protected Transform rightController;
    protected VRTK_ControllerEvents leftEvents;
    protected VRTK_ControllerEvents rightEvents;
    protected bool _isGrabbed;
    protected DirectionAttraction dAttraction;
    protected SelectInjection sInjection;
    protected bool _hasChosen;
    protected bool _objectIsHuman;
    public List<Medicine> pulledMedicine = new List<Medicine>(); //list that keeps track of all the medication that's pulled in syringe (only clear when value is 0.00)
    protected Medicine _currentCollidingMedicine;
    //protected bool toggle;

    const string NEEDLELAYER = "needle";
    const string INSIDESYRINGE = "inside";
    const string FILLWATER = "fillWater";
    const string VRTKSCRIPT = "vrtk_scripts";
    const string LCONTR = "LeftController";
    const string RCONTR = "RightController";

    void Start()
    {
        _objectIsHuman = false;
        _hasChosen = false;
		string temp = "34.50";
        isPulling = false;
        //toggle = false;
        GameObject vrtkScripts = GameObject.FindGameObjectWithTag(VRTKSCRIPT);
        leftController = vrtkScripts.transform.Find(LCONTR);
        rightController = vrtkScripts.transform.Find(RCONTR);
        leftEvents = leftController.GetComponent<VRTK_ControllerEvents>();
        rightEvents = rightController.GetComponent<VRTK_ControllerEvents>();
        sInjection = leftController.GetComponent<SelectInjection>(); //the selectionInjection script should be always and ONLY on the left controller

        snapDrop = this.GetComponentInChildren<VRTK_SnapDropZone>().transform;
        lcdText = this.GetComponentInChildren<Text>();
        lcdCanvas = this.GetComponentInChildren<Canvas>().transform;
        insideSyringe = this.transform.GetChild(0).Find(INSIDESYRINGE);
        fillWater = this.transform.GetChild(0).Find(FILLWATER);
        dAttraction = this.transform.GetChild(0).GetComponent<DirectionAttraction>();
        beginPosition = insideSyringe.localPosition;
        beginPositionWater = fillWater.localPosition;
        interactScript = GetComponent<VRTK_InteractableObject>();

        lcdCanvas.gameObject.SetActive(false);

        interactScript.InteractableObjectUsed += new InteractableObjectEventHandler(ObjectUsed);
        interactScript.InteractableObjectUnused += new InteractableObjectEventHandler(ObjectUnused);
        interactScript.InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
        interactScript.InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectUngrabbed);

        leftEvents.TouchpadPressed += new ControllerInteractionEventHandler(LeftTouchpadPressed);
        leftEvents.TouchpadReleased += new ControllerInteractionEventHandler(LeftTouchpadReleased);
        rightEvents.TouchpadPressed += new ControllerInteractionEventHandler(RightTouchpadPressed);
        rightEvents.TouchpadReleased += new ControllerInteractionEventHandler(RightTouchpadReleased);
   }

    public bool IsGrabbedWithNeedle
    {
        get{
            return _isGrabbed&&HasNeedle();
        }
    }

    public bool ObjectIsHuman
    {
        get{
            return _objectIsHuman;
        }
        set{
            _objectIsHuman = value;
        }
    }

    public Medicine CurrentlCollidingMedicine
    {
        get{
            return _currentCollidingMedicine;
        }
        set{
            _currentCollidingMedicine = value;
        }
    }

    protected void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("Is grabbing");
        _isGrabbed = true;
        if (leftController.GetComponent<VRTK_InteractGrab>().GetGrabbedObject() == this.gameObject)
        {
            grabbedByLeftHand = true;
            leftController.GetComponent<VRTK_Pointer>().enabled = false;
			//Debug.Log ("left pointer disabled");
        }
        else
        {
            grabbedByLeftHand = false;
            rightController.GetComponent<VRTK_Pointer>().enabled = false;
			//Debug.Log ("right pointer disabled");
        }
    }

    protected void ObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("Is no more grabbing");
        _isGrabbed = false;
        rightController.GetComponent<VRTK_Pointer>().enabled = true;
        leftController.GetComponent<VRTK_Pointer>().enabled = true;
        _hasChosen = false; //resets the fact that you already chose an injection option;
		//Debug.Log ("pointers enabled");
    }

    protected void LeftTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (leftController.GetComponent<VRTK_InteractGrab>().GetGrabbedObject() == this.gameObject)
        {
            ObjectTouchPad();
        }
    }
    protected void LeftTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (leftController.GetComponent<VRTK_InteractGrab>().GetGrabbedObject() == this.gameObject)
        {
            ObjectUntouchPad();
        }
    }
    protected void RightTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (rightController.GetComponent<VRTK_InteractGrab>().GetGrabbedObject() == this.gameObject)
        {
            ObjectTouchPad();
        }
    }
    protected void RightTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (rightController.GetComponent<VRTK_InteractGrab>().GetGrabbedObject() == this.gameObject)
        {
            ObjectUntouchPad();
        }
    }

    public bool HasChosen
    {
        get { return _hasChosen; }
        set { _hasChosen = value; }
    }

    public void StopChoosing()
    {
        HasChosen = true;
        if (grabbedByLeftHand)
        {
            rightController.GetComponent<VRTK_Pointer>().enabled = true;
        }
        else
        {
            leftController.GetComponent<VRTK_Pointer>().enabled = true;
        }
    }

    public void SelectInjectionMethod()
    {
        if (HasNeedle())
        {
            _objectIsHuman = true;
            sInjection.Subscribe(this);
            sInjection.optionChosen = false;
            if (grabbedByLeftHand)
            {
                sInjection.EnableLeftOptions(false); //reversed because when grabbing with left hand your controller dissapears so you need to show it on the right controller
                sInjection.EnableRightOptions(true);
                sInjection.leftHand = false;
                rightController.GetComponent<VRTK_Pointer>().enabled = false;
            }
            else
            {
                sInjection.EnableRightOptions(false);
                sInjection.EnableLeftOptions(true);
                sInjection.leftHand = true;
                leftController.GetComponent<VRTK_Pointer>().enabled = false;
            }
        }
    }

    public void ObjectTouchPad()
    {
        if (HasNeedle() && !isPulling && !isPushing && dAttraction.IsCollidingWithInjectionZone && HasChosen)
        {
            isPushing = true;
            StartCoroutine(Pushing());
        }       
    }

    public void ObjectUntouchPad()
    {
        isPushing = false;
    }


    private void ObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        if (HasNeedle() && !isPulling && !isPushing && dAttraction.IsCollidingWithInjectionZone && HasChosen && !_objectIsHuman) //can't pull if object is human!
        {
            isPulling = true;
            StartCoroutine(Pulling());
            //ADD medicine to list
            if (!pulledMedicine.Contains(_currentCollidingMedicine))
            {
                pulledMedicine.Add(_currentCollidingMedicine);
            }

        }
    }

    private void ObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        isPulling = false;
    }

    protected void ResizeWater(float distance)
    {
        fillWater.localPosition = new Vector3(beginPositionWater.x, beginPositionWater.y, beginPositionWater.z - (distance/2));
        fillWater.localScale = new Vector3(fillWater.localScale.x, distance / 2, fillWater.localScale.z);
        lcdCanvas.gameObject.SetActive(true);
		float accurateValue = (distance / maxMove) * syringeValue;
		float valueF = ((Mathf.Round (accurateValue * 2)) / 2.0f);
		string value = valueF.ToString ("F2");

        if (value == "0.00") //if syringe is empty clear medication it has pulled
        {
            pulledMedicine.Clear();
            Debug.Log("Medication in syringe cleared!");
        }

        lcdText.text = value + " ml";

		if (float.Parse(value)%1 == 0) //this should buzz every ml
        {
            VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(GetUsedHand()), 0.5f, 0.2f, 0.5f);
        }
    }

    protected bool HasNeedle()
    {
        bool returnValue = false;
        foreach (Transform child in snapDrop)
        {
            if (LayerMask.LayerToName(child.gameObject.layer) == NEEDLELAYER)
            {
                returnValue = true;
            }
        }
        return returnValue;
    }

    protected SDK_BaseController.ControllerHand GetUsedHand()
    {
        if (grabbedByLeftHand)
        {
            return SDK_BaseController.ControllerHand.Left;
        }
        else
        {
            return SDK_BaseController.ControllerHand.Right;
        }
    }

    IEnumerator Pulling()
    {
        while (!isPushing && isPulling && (beginPosition.z- insideSyringe.localPosition.z) < maxMove) 
        {
            yield return new WaitForEndOfFrame();
            float distance = (beginPosition.z - insideSyringe.localPosition.z);
            ResizeWater(distance);
            insideSyringe.localPosition -= (Vector3.forward * Time.deltaTime * speed);
        }
        isPulling = false;
    }

    IEnumerator Pushing()
    {
        while (!isPulling && isPushing && (beginPosition.z- insideSyringe.localPosition.z) > 0)
        {
            yield return new WaitForEndOfFrame();
            float distance = (beginPosition.z - insideSyringe.localPosition.z);
            ResizeWater(distance);
            insideSyringe.localPosition += (Vector3.forward * Time.deltaTime * speed);
            
        }
        isPushing = false;
    }
}
