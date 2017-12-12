using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderIgnorer : MonoBehaviour {
	public int layer1;
	public int layer2;

	// Use this for initialization
	void Start () {
		Physics.IgnoreLayerCollision (layer1, layer2, true);
	}
}
