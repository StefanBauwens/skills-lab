using System;
using UnityEngine;
using System.Collections;

namespace ZenFulcrum.EmbeddedBrowser {

public class Door : MonoBehaviour {
	public Vector3 openOffset = new Vector3(0, -6.1f, 0);
	[Tooltip("Time to open or close, in seconds.")]
	public float openSpeed = 2;

	[Tooltip("Number of coins needed to open the door.")]
	public int numCoins = 0;

	private Vector3 closedPos, openPos;

	public enum OpenState {
		Open, Closed, Opening, Closing
	}

	public event Action<OpenState> stateChange = state => {};

	private OpenState _state;
	public OpenState State {
		get { return _state; }
		set {
			_state = value;
			stateChange(_state);
		}
	}

	public void Start() {
		closedPos = transform.position;
		openPos = transform.position + openOffset;
		State = OpenState.Closed;

		var browser = GetComponentInChildren<Browser>();

		//Tell the interface how many coins we need
		browser.CallFunction("setRequiredCoins", numCoins);

		browser.RegisterFunction("toggleDoor", args => {
			switch ((string)args[0].Check()) {
				case "open": Open(); break;
				case "close": Close(); break;
				case "toggle": Toggle(); break;
			}
		});

		//Update interface when we get a coin
		PlayerInventory.Instance.coinCollected += coinCount => {
			browser.CallFunction("setCoinCoint", coinCount);
		};
	}

	/** Toggles open state. */
	public void Toggle() {
		if (State == OpenState.Open || State == OpenState.Opening) Close();
		else Open();
	}

	public void Open() {
		if (State == OpenState.Open) return;
		State = OpenState.Opening;
	}

	public void Close() {
		if (State == OpenState.Closed) return;
		State = OpenState.Closing;
	}

	public void Update() {
		if (State == OpenState.Opening) {
			var percent = Vector3.Distance(transform.position, closedPos) / openOffset.magnitude;
			percent = Mathf.Min(1, percent + Time.deltaTime / openSpeed);
			transform.position = Vector3.Lerp(closedPos, openPos, percent);
			if (percent >= 1) State = OpenState.Open;
		} else if (State == OpenState.Closing) {
			var percent = Vector3.Distance(transform.position, openPos) / openOffset.magnitude;
			percent = Mathf.Min(1, percent + Time.deltaTime / openSpeed);
			transform.position = Vector3.Lerp(openPos, closedPos, percent);
			if (percent >= 1) State = OpenState.Closed;
		}
	}
}

}
