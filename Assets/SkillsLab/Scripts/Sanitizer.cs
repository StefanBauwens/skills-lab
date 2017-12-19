using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanitizer : MonoBehaviour {
	const float ammountToMove = 0.009f;
	const float factor = 0.01f;
	protected Vector3 originalPosition;
	static bool coroutineRunning;

	// Use this for initialization
	void Start () {
		originalPosition = this.transform.position;
	}

	void OnTriggerEnter(Collider other)
	{
		if (!coroutineRunning) {
			coroutineRunning = true;
			StartCoroutine (Push ());
		}
	}

	IEnumerator Push()
	{
		while (this.transform.position.y > originalPosition.y-ammountToMove) {
			yield return new WaitForEndOfFrame ();
			this.transform.position -= new Vector3 (0, factor*Time.fixedDeltaTime, 0);
		}
		yield return new WaitForSeconds (0.5f);
		StartCoroutine (GoUp ());
	}

	IEnumerator GoUp()
	{
		while (this.transform.position.y < originalPosition.y) {
			yield return new WaitForEndOfFrame ();
			this.transform.position += new Vector3 (0, factor*Time.fixedDeltaTime, 0);
		}
		coroutineRunning = false;
	}
}
