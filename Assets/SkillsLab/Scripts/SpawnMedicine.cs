using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SpawnMedicine : MonoBehaviour {

	private VRTK_InteractableObject interactScript;
	public GameObject objectToSpawn;

	void Start()
	{
		interactScript = GetComponent<VRTK_InteractableObject>();
		interactScript.InteractableObjectUsed += new InteractableObjectEventHandler(ObjectUsed);
		interactScript.InteractableObjectUnused += new InteractableObjectEventHandler(ObjectUnused);

	}

	private void ObjectUsed(object sender, InteractableObjectEventArgs e)
	{
		Instantiate (objectToSpawn, this.gameObject.transform.position, Quaternion.identity);
	}

	private void ObjectUnused(object sender, InteractableObjectEventArgs e)
	{	
		Instantiate (objectToSpawn, this.gameObject.transform.position, Quaternion.identity);
	}
}
