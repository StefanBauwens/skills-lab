using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {

public class PlayerInventory : MonoBehaviour {
	public HUDManager hud;
	public static PlayerInventory Instance { get; private set; }
	public int NumCoins { get; private set; }

	public event Action<int> coinCollected = coins => { };

	public void Awake() {
		Instance = this;
	}

	public void AddCoin() {
		NumCoins += 1;

		coinCollected(NumCoins);
	}
}

}
