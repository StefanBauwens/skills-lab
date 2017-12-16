using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectionZone : NaaldContainer {

	// Use this for initialization
	void Start () {
        material = this.gameObject.GetComponentInChildren<MeshRenderer>().material;
        material.color = standardColor;
	}

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == needleLayer)
        {
            material.color = standardColor;
        }
    }
}
