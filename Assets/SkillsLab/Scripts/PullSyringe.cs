using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class PullSyringe : MonoBehaviour {

    private VRTK_InteractableObject interactScript;
    protected Transform snapDrop;
    protected Transform insideSyringe;
    protected Transform fillWater;
    public float speed = 0.1f;
    public float maxMove = 0.1f;
    public float hapticInterval = 0.02f;
    public float hapticIntervalError = 0.005f;
    public float syringeValue = 10; //10ml
    protected Vector3 beginPosition;
    protected Vector3 beginPositionWater;
    protected bool isPulling;
    protected bool isPushing;
    protected Text lcdText;
    protected Transform lcdCanvas;
    //protected bool toggle;

    const string NEEDLELAYER = "needle";
    const string INSIDESYRINGE = "inside";
    const string FILLWATER = "fillWater";

    void Start()
    {
        isPulling = false;
        //toggle = false;
        snapDrop = this.GetComponentInChildren<VRTK_SnapDropZone>().transform;
        //lcdText = this.GetComponentInChildren<Text>();
        //lcdCanvas = this.GetComponentInChildren<Canvas>().transform;
        insideSyringe = this.transform.Find(INSIDESYRINGE);
        //fillWater = this.transform.Find(FILLWATER);
        beginPosition = insideSyringe.localPosition;
        //beginPositionWater = fillWater.localPosition;
        interactScript = GetComponent<VRTK_InteractableObject>();

        //lcdCanvas.gameObject.SetActive(false);

        interactScript.InteractableObjectUsed += new InteractableObjectEventHandler(ObjectUsed);
        interactScript.InteractableObjectUnused += new InteractableObjectEventHandler(ObjectUnused);

        interactScript.InteractableObjectTouched += new InteractableObjectEventHandler(ObjectTouched);
        interactScript.InteractableObjectUntouched += new InteractableObjectEventHandler(ObjectUntouched);
    }

    private void ObjectTouched(object sender, InteractableObjectEventArgs e)
    {
        foreach (Transform child in snapDrop)
        {
            if (LayerMask.LayerToName(child.gameObject.layer) == NEEDLELAYER && !isPulling && !isPulling)
            {
                isPushing = true;
                StartCoroutine(Pushing());
            }
        }
    }

    private void ObjectUntouched(object sender, InteractableObjectEventArgs e)
    {
        isPushing = false;
    }


    private void ObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        foreach (Transform child in snapDrop)
        {
            if (LayerMask.LayerToName(child.gameObject.layer) == NEEDLELAYER && !isPulling && !isPulling)
            {
                //if (!toggle)
                //{
                    isPulling = true;
                    StartCoroutine(Pulling());
                /*}
                else
                {
                    isPushing = true;
                    StartCoroutine(Pushing());
                }*/
               
            }
        }
    }

    private void ObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        //toggle = !toggle;
        isPulling = false;
        //isPushing = false;
    }

    protected void ResizeWater(float distance)
    {
        /*fillWater.position = new Vector3(beginPositionWater.x, beginPositionWater.y+(distance/2), beginPositionWater.z);
        fillWater.localScale = new Vector3(fillWater.localScale.x, distance, fillWater.localScale.z);
        lcdCanvas.gameObject.SetActive(true);
        lcdText.text = ((distance / maxMove) * syringeValue) + " ml";*/
    }

    IEnumerator Pulling()
    {
        while (!isPushing && isPulling && (insideSyringe.localPosition.y - beginPosition.y) < maxMove) 
        {
            yield return new WaitForEndOfFrame();

            float distance = (insideSyringe.localPosition.y - beginPosition.y);
            ResizeWater(distance);
            /*if (distance % hapticInterval < hapticIntervalError)
            {
                //!! CHANGE HAND TO CURREN THAND GRABBING
                VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(SDK_BaseController.ControllerHand.Right), 0.5f, 0.2f, 0.5f);
            }*/

            insideSyringe.localPosition += (Vector3.up * Time.deltaTime * speed);
        }
        isPulling = false;
        //lcdCanvas.gameObject.SetActive(false);
    }

    IEnumerator Pushing()
    {
        while (!isPulling && isPushing && (insideSyringe.localPosition.y - beginPosition.y) > 0)
        {
            yield return new WaitForEndOfFrame();

            float distance = (insideSyringe.localPosition.y - beginPosition.y);
            ResizeWater(distance);

            /*if (distance%hapticInterval < hapticIntervalError)
            {
                //!! CHANGE HAND TO CURREN THAND GRABBING
                VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(SDK_BaseController.ControllerHand.Right), 0.5f, 0.2f, 0.5f);
            }*/
            insideSyringe.localPosition -= (Vector3.up * Time.deltaTime * speed);
            
        }
        isPushing = false;
        //lcdCanvas.gameObject.SetActive(false);
    }
}
