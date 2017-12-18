using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaaldContainer : MonoBehaviour {
    public string needleLayer = "needle";
    public Color standardColor = new Color(255, 0, 0, 0);
    public Color snapColor = new Color(255, 0, 0, 240);
    protected Material material;

	// Use this for initialization
	void Start () {
        material = this.gameObject.GetComponent<MeshRenderer>().material;
        material.color = standardColor;
	}

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Object in trigger is a needle
        if (LayerMask.LayerToName(other.gameObject.layer)==needleLayer)
        {
            material.color = snapColor;
        }
    }

    protected virtual void OnTriggerExit(Collider other) //only remove needle after pulling it back out
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == needleLayer)
        {
            material.color = standardColor;
            Destroy(other.gameObject);
        }
    }
}
