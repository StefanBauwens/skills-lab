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
    public GameObject particleTablet;

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
                child.GetComponent<BoxCollider>().enabled = false;
                child.GetComponent<VRTK_InteractableObject>().isGrabbable = false;
                Destroy(child.GetComponent<Rigidbody>());
                child.position = particleTablet.transform.position;
                this.transform.parent.GetComponent<MedicineData>().medicine = child.transform.GetComponent<MedicineData>().medicine; //copies medicine data from effervescenttablet to cup
                Debug.Log("Medicine tablet: " + child.transform.GetComponent<MedicineData>().medicine.mName + " medicine put on cup: " + this.transform.parent.GetComponent<MedicineData>().medicine.mName);
                StartCoroutine(PlayParticleTablet(child));
            }
        }
    }

    private IEnumerator PlayParticleTablet(Transform tablet)
    {
        particleTablet.SetActive(true);
        yield return new WaitForSeconds(7);
        // Change to medicineWater texture
        waterMaterial.mainTexture = dissolvedWater;
        tabletInWater = true;
        Destroy(particleTablet);
        Destroy(this.gameObject);
    }
}
