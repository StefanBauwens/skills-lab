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

    // Syringe enters injection zone
    protected override void OnTriggerEnter(Collider other)
    {
        // Change cylinder to snap color
        base.OnTriggerEnter(other);

        // Disable clothing piece
        switch (clothingPiece)
        {
            case Clothing.Top:
                top.SetActive(false);
                break;
            case Clothing.Bottom:
                bottom.SetActive(false);
                break;
            case Clothing.Both:
                top.SetActive(false);
                bottom.SetActive(false);
                break;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == needleLayer)
        {
            material.color = standardColor;
        }
    }
}
