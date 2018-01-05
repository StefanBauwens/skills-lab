using UnityEngine;
using System.Collections;

namespace ZenFulcrum.EmbeddedBrowser { 

/** Out of the box, our BrowserAssets won't be in place. If they aren't, explain what to do. */
[RequireComponent(typeof(Browser))]
public class ExplainUnzip : MonoBehaviour {

	public void Start() {
		var browser = GetComponent<Browser>();

		browser.onFetch += data => {
			if (data["status"] == 404) {
				browser.LoadHTML(Resources.Load<TextAsset>("ExplainUnzip").text);
				if (HUDManager.Instance) HUDManager.Instance.Pause();
				Time.timeScale = 1;
			}
		};

		browser.onFetchError += data => {
			//For abysmally slow computers:
			if (data["error"] == "ERR_ABORTED") {
				browser.QueuePageReplacer(() => {}, 1);
			}
		};

	}
}

}
