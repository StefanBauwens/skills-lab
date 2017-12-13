using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PullSyringe : MonoBehaviour {

    private VRTK_InteractableObject interactScript;
    protected Transform snapDrop;
    protected Transform insideSyringe;
    public float speed = 0.1f;
    public float maxMove = 0.1f;
    public float hapticInterval = 0.02f;
    public float hapticIntervalError = 0.005f;
    protected Vector3 beginPosition;
    protected bool isPulling;
    protected bool isPushing;
    protected bool toggle;

    const string NEEDLELAYER = "needle";
    const string INSIDESYRINGE = "inside";

    void Start()
    {
        isPulling = false;
        toggle = false;
        snapDrop = this.GetComponentInChildren<VRTK_SnapDropZone>().transform;
        insideSyringe = this.transform.Find(INSIDESYRINGE);
        beginPosition = insideSyringe.transform.localPosition;
        interactScript = GetComponent<VRTK_InteractableObject>();

        interactScript.InteractableObjectUsed += new InteractableObjectEventHandler(ObjectUsed);
        interactScript.InteractableObjectUnused += new InteractableObjectEventHandler(ObjectUnused);
    }

    private void ObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        foreach (Transform child in snapDrop)
        {
            if (LayerMask.LayerToName(child.gameObject.layer) == NEEDLELAYER && !isPulling && !isPulling)
            {
                if (!toggle)
                {
                    isPulling = true;
                    StartCoroutine(Pulling());
                }
                else
                {
                    isPushing = true;
                    StartCoroutine(Pushing());
                }
               
            }
        }
    }

    private void ObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        toggle = !toggle;
        isPulling = false;
        isPushing = false;
    }

    IEnumerator Pulling()
    {
        while (!isPushing && isPulling && (insideSyringe.localPosition.y - beginPosition.y) < maxMove) 
        {
            yield return new WaitForEndOfFrame();

            float distance = (insideSyringe.localPosition.y - beginPosition.y);
            if (distance % hapticInterval < hapticIntervalError)
            {
                //!! CHANGE HAND TO CURREN THAND GRABBING
                VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(SDK_BaseController.ControllerHand.Right), 0.5f, 0.2f, 0.5f);
            }

            insideSyringe.localPosition += (Vector3.up * Time.deltaTime * speed);
        }
        isPulling = false;
    }

    IEnumerator Pushing()
    {
        while (!isPulling && isPushing && (insideSyringe.localPosition.y - beginPosition.y) > 0)
        {
            yield return new WaitForEndOfFrame();

            float distance = (insideSyringe.localPosition.y - beginPosition.y);
            if (distance%hapticInterval < hapticIntervalError)
            {
                //!! CHANGE HAND TO CURREN THAND GRABBING
                VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(SDK_BaseController.ControllerHand.Right), 0.5f, 0.2f, 0.5f);
            }
            insideSyringe.localPosition -= (Vector3.up * Time.deltaTime * speed);
            
        }
        isPushing = false;
    }
}
