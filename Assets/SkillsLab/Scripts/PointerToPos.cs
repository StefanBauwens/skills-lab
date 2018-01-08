using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using ZenFulcrum.EmbeddedBrowser;

//By Stefan Bauwens 
public class PointerToPos : MonoBehaviour {
	public VRTK_Pointer leftPointer; //don't forget to attach pointer
	public VRTK_Pointer rightPointer; 

	public Transform browserGUI;

	protected float width;
	protected float height;

	void Start () {
		width = browserGUI.GetComponent<RectTransform> ().rect.width;
		height = browserGUI.GetComponent<RectTransform> ().rect.height;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log( "LocalPosition:" +GetLocalPosition());
	}

	Vector3 GetPosition()
	{
		Vector3 position = new Vector3(-1,-1,-1);
		if (leftPointer.IsPointerActive ()) {
			position = leftPointer.pointerRenderer.GetDestinationHit ().point;
		} else if (rightPointer.IsPointerActive ()) {
			position = rightPointer.pointerRenderer.GetDestinationHit ().point;
		}
		return position;
	}

	public Vector2 GetLocalPosition() 
	{
		Vector3 position = GetPosition (); //gets world position
		Vector2 locPosition = new Vector2(-1,-1);
		if (position != new Vector3(-1,-1,-1)) {
			locPosition = browserGUI.InverseTransformPoint (position) + new Vector3(width/2, height/2, 0);

			//truncate
			if (locPosition.x < 0) {
				locPosition = new Vector2 (0, locPosition.y);
			} else if (locPosition.x > width) {
				locPosition = new Vector2 (width, locPosition.y);
			}
			if (locPosition.y < 0) {
				locPosition = new Vector2 (locPosition.x, 0);
			} else if (locPosition.y > height) {
				locPosition = new Vector2 (locPosition.x, height);
			}

			//locPosition = new Vector2 (locPosition.x / width, locPosition.y / height);
		}

		return locPosition;
	}

	public Vector2 GetLocalPosition2() //value between (0,0) and (1,1)
	{
		Vector2 locPosition = GetLocalPosition ();
		if (locPosition != new Vector2(-1,-1)) {
			locPosition = new Vector2 (locPosition.x / width, locPosition.y / height);
		}
		return locPosition;
	}
		
}
