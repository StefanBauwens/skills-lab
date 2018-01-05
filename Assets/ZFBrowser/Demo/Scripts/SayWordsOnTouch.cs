using System;
using System.Collections;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {
/** Says something(s) in the HUD when the user touches our collider (one-shot). */
public class SayWordsOnTouch : MonoBehaviour {

	public static int ActiveSpeakers { get; private set; }

	[Serializable]
	public class Verse {
		/** How long since the touch/last saying should we wait? */
		public float delay;
		/** What should we say? Full HTML support (so mind the security implications). */
		[Multiline]
		public string textHTML;

		public float dwellTime = 5;
	}

	public Verse[] thingsToSay;
	private bool triggered, stillTriggered;
	/** Make our box collider this much bigger when we enter so it's harder to leave. */
	public float extraLeaveRange = 0;

	public void OnTriggerEnter(Collider other) {
		if (triggered) return;
		var inventory = other.GetComponent<PlayerInventory>();
		if (!inventory) return;

		triggered = true;
		stillTriggered = true;
		++ActiveSpeakers;

		StartCoroutine(SayStuff());

		var bc = GetComponent<BoxCollider>();
		if (bc) {
			var size = bc.size;
			size.x += extraLeaveRange * 2;
			size.y += extraLeaveRange * 2;
			size.z += extraLeaveRange * 2;
			bc.size = size;
		}
	}

	private IEnumerator SayStuff() {
		for (int idx = 0; idx < thingsToSay.Length && stillTriggered; ++idx) {
			yield return new WaitForSeconds(thingsToSay[idx].delay);
			if (!stillTriggered) break;
			HUDManager.Instance.Say(thingsToSay[idx].textHTML, thingsToSay[idx].dwellTime);
		}
		--ActiveSpeakers;
		Destroy(gameObject);
	}

	public void OnTriggerExit(Collider other) {
		var inventory = other.GetComponent<PlayerInventory>();
		if (!inventory) return;

		stillTriggered = false;
	}


}

}
