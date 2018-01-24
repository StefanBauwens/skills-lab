
using System.Collections;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {

/** Game-specific logic for picking up coins. */
public class CoinPickup : MonoBehaviour {
	private Transform coinVis;

	public float spinSpeed = 20;
	public bool isMassive = false;

	public void Start() {
		coinVis = transform.Find("Vis");
	}

	public void Update() {
		coinVis.transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * spinSpeed, Vector3.up);
	}

	public void OnTriggerEnter(Collider other) {
		var inventory = other.GetComponent<PlayerInventory>();
		if (!inventory) return;

		if (isMassive) {
			HUDManager.Instance.LoadBrowseLevel();
		} else {
			inventory.AddCoin();
		}

		Destroy(gameObject);
	}

}

}
