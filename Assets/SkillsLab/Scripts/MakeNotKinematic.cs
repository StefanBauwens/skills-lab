using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeNotKinematic : MonoBehaviour {

	protected const float WAITTIME = 5;

	// Use this for initialization
	void Start () {
		StartCoroutine (KinematicFalse());
	}

	IEnumerator KinematicFalse()
	{
		yield return new WaitForSeconds (WAITTIME);
		this.gameObject.GetComponent<Rigidbody> ().isKinematic = false;
	}
}
