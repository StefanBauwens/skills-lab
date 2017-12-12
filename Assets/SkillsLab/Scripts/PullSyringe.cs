using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PullSyringe : MonoBehaviour {

    private VRTK_InteractableObject interactScript;
    protected Transform snapDrop;
    protected Transform insideSyringe;
    public float ammountToMove = 0.1f;
    public float maxMove = 0.1f;
    protected Vector3 beginPosition;
    protected bool isPulling;

    const string NEEDLELAYER = "needle";
    const string INSIDESYRINGE = "inside";

    void Start()
    {
        isPulling = false;
        snapDrop = this.GetComponentInChildren<VRTK_SnapDropZone>().transform;
        insideSyringe = this.transform.Find(INSIDESYRINGE);
        beginPosition = insideSyringe.transform.localPosition;
        interactScript = GetComponent<VRTK_InteractableObject>();
        interactScript.InteractableObjectUsed += new InteractableObjectEventHandler(ObjectUsed);
        interactScript.InteractableObjectUnused += new InteractableObjectEventHandler(ObjectUnused);
    }

    private void ObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("You're clicking");

        foreach (Transform child in snapDrop)
        {
            if (LayerMask.LayerToName(child.gameObject.layer) == NEEDLELAYER && !isPulling)
            {
                isPulling = true;
                StartCoroutine(Pulling());
            }
        }
    }

    private void ObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        isPulling = false;
        Debug.Log("You stopped clicking");
    }

    IEnumerator Pulling()
    {
        Debug.Log("Distance = " + (insideSyringe.transform.localPosition.y - beginPosition.y));
        while (isPulling && (insideSyringe.localPosition.y - beginPosition.y) < maxMove) 
        {
            yield return new WaitForEndOfFrame();
            insideSyringe.localPosition += (Vector3.up * Time.deltaTime * ammountToMove);
        }
        isPulling = false;
    }

}
