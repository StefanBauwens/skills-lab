using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloner : MonoBehaviour {
	public Vector3 copiesInDimensions;
	public GameObject objectToCopy;
	public float offsetX;
	public float offsetY;
	public float offsetZ;


	// Use this for initialization
	void Awake () {
		for (int x = 1; x < copiesInDimensions.x; x++) {
			for (int y = 1; y < copiesInDimensions.y; y++) {
				for (int z = 1; z < copiesInDimensions.z; z++) {
					Instantiate (objectToCopy, objectToCopy.transform.position + new Vector3 (x *offsetX, y *offsetY, z *offsetZ), objectToCopy.transform.rotation, objectToCopy.transform.parent);
				}
			}
		}
	}
}
