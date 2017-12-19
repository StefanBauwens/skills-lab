using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DestroyMedicine : MonoBehaviour {

	protected VRTK_SnapDropZone snapDropZone;
	const string TAG = "medicine";

    void Start()
    {
        snapDropZone = this.gameObject.GetComponent<VRTK_SnapDropZone>();

        snapDropZone.ObjectSnappedToDropZone += new SnapDropZoneEventHandler(DestroyMed);
    }

	protected void DestroyMed(object sender, SnapDropZoneEventArgs e)
	{
		Debug.Log ("Object : " + sender);
		foreach (Transform child in this.transform) {
			if (child.tag == TAG) {
				Destroy (child.gameObject);
			}
		}
        //GameObject.Destroy ((GameObject)sender);
	}
}
