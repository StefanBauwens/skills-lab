using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ZenFulcrum.EmbeddedBrowser {

/** When our trigger is touched by the player, does the actions in the list. (one-shot) */
public class ActionTimer : MonoBehaviour {

	[Serializable]
	public class TimedAction {
		/** How long since the action should we wait? */
		public float delay;
		/** What action should we take? */
		public UnityEvent action;
	}

	public TimedAction[] thingsToDo;
	private bool triggered;

	public void OnTriggerEnter(Collider other) {
		if (triggered) return;
		var inventory = other.GetComponent<PlayerInventory>();
		if (!inventory) return;

		triggered = true;

		StartCoroutine(DoThings());
	}

	private IEnumerator DoThings() {
		for (int idx = 0; idx < thingsToDo.Length; ++idx) {
			yield return new WaitForSeconds(thingsToDo[idx].delay);
			thingsToDo[idx].action.Invoke();
		}
	}
}

}
