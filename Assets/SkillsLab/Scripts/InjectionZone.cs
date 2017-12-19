using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Clothing { Top, Bottom, Both};

public class InjectionZone : NaaldContainer {

    public Clothing clothingPiece;
    public GameObject top;
    public GameObject bottom;

    // Use this for initialization
    void Start () {  
        material = this.gameObject.GetComponentInChildren<MeshRenderer>().material;
        material.color = standardColor;
	}

	protected void toggleClothes(bool enabled)
	{
		switch (clothingPiece)
		{
			case Clothing.Top:
				top.SetActive(enabled);
				break;
			case Clothing.Bottom:
				bottom.SetActive(enabled);
				break;
			case Clothing.Both:
				top.SetActive(enabled);
				bottom.SetActive(enabled);
				break;
		}
	}

    // Syringe enters injection zone
    protected override void OnTriggerEnter(Collider other)
    {
        // Change cylinder to snap color
        base.OnTriggerEnter(other);

        // Object in trigger is a needle
        if (LayerMask.LayerToName(other.gameObject.layer) == needleLayer)
        {
            // Disable clothing piece
			toggleClothes(false);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == needleLayer)
        {
            material.color = standardColor;
			toggleClothes (true);
        }
    }
}
