using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DestroyMedicine : MonoBehaviour {

    protected VRTK_SnapDropZone snapDropZone;

    void Start()
    {
        snapDropZone = this.gameObject.GetComponent<VRTK_SnapDropZone>();

        snapDropZone.ObjectSnappedToDropZone += new SnapDropZoneEventHandler(DestroyMed);
    }

	protected void DestroyMed(object sender, SnapDropZoneEventArgs e)
	{
        GameObject.Destroy ((GameObject)sender);
	}
}
