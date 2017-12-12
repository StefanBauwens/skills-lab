using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Tablet : MonoBehaviour {
	public ChangeUIPointer changePointerScript;
	private VRTK_InteractableObject interactScript;

	void Start()
	{
		interactScript = GetComponent<VRTK_InteractableObject>();
		interactScript.InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
		interactScript.InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectUngrabbed);
	}

	private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
	{
		changePointerScript.SetPointerRenderer (false);
	}

	private void ObjectUngrabbed(object sender, InteractableObjectEventArgs e)
	{
		changePointerScript.SetPointerRenderer (true);
	}
}
