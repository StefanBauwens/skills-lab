using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RotateInventoryObject : MonoBehaviour {

    private VRTK_SnapDropZone snapdropScript;
    private Vector3 realScaleTablet;

    // Use this for initialization
    void Start () {
        snapdropScript = GetComponent<VRTK_SnapDropZone>();
        snapdropScript.ObjectSnappedToDropZone += new SnapDropZoneEventHandler(ItemSnapped);
    }

    // when item is snapped to inventory
    private void ItemSnapped(object sender, SnapDropZoneEventArgs e)
    {
        foreach(Transform child in this.transform)
        {
            if (child.GetComponent<RotationVectorInventory>())
            {
                child.Rotate(child.GetComponent<RotationVectorInventory>().rotationInInventory);
            }
            //{
                //// Item is tablet
                //if (child.name.Contains("realTablet"))
                //{
                //    Debug.Log("Found tablet: " + child.name);
                //    child.Rotate(new Vector3(-90, 90, 0));
                //}
                //else if (child.name.Contains("Syringe"))
                //{
                //    child.Rotate(new Vector3(0, 0, -90));
                //}
                //else if (child.name.Contains("Cup") || child.name.Contains("Badge"))
                //{
                //    child.Rotate(new Vector3(-90, 0, 0));
                //}
            //}
        }
    }
}
