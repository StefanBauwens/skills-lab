using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DissolveEffervescentTablet : MonoBehaviour {

    private VRTK_SnapDropZone snapdropScript;
    public GameObject waterObject;
    private Material waterMaterial;
    public Texture dissolvedWater;
    public bool tabletInWater;

    // Use this for initialization
    void Start () {
        snapdropScript = GetComponent<VRTK_SnapDropZone>();
        waterMaterial = waterObject.GetComponent<MeshRenderer>().material;
        snapdropScript.ObjectSnappedToDropZone += new SnapDropZoneEventHandler(DissolveTablet);
	}

    // When tablet is snapped to cup of water
    private void DissolveTablet(object sender, SnapDropZoneEventArgs e)
    {
        foreach (Transform child in this.transform)
        {
            if(child.tag == "effervescentTablet")
            {
                // ADD Splash particles
                // Change to medicineWater texture
                waterMaterial.mainTexture = dissolvedWater;
                Destroy(child.gameObject);
                tabletInWater = true;
                Destroy(this.gameObject);
            }
        }
    }
}
