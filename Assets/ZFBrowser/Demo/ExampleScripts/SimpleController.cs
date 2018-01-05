using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

/**
 * A very simple controller for a Browser.
 * Call GoToURLInput() to go to the URL typed in urlInput
 */
[RequireComponent(typeof(Browser))]
public class SimpleController : MonoBehaviour {

	private Browser browser;
	public InputField urlInput;

	public void Start() {
		browser = GetComponent<Browser>();
	}

	public void GoToURLInput() {
		browser.Url = urlInput.text;
	}

}

