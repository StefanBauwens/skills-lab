using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {

	/**
	 * Attach this to a browser.
	 * When it starts up, it will register itself as the NewWindowHandler on the browser.
	 * 
	 * When a new window is opened, it will create a ball to show that new window's contents, and drop it 
	 * from {spawnPosition}.
	 */
	[RequireComponent(typeof(Browser))]
public class BallBrowserSpawner : MonoBehaviour, INewWindowHandler {

	public Transform spawnPosition;
	public float size;

	public void Start() {
		GetComponent<Browser>().NewWindowHandler = this;
	}

	public Browser CreateBrowser(Browser parent) {
		var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		ball.AddComponent<Rigidbody>();
		ball.transform.localScale = new Vector3(size, size, size);
		ball.transform.position = spawnPosition.position + Vector3.one * Random.value * .01f;

		var browser = ball.AddComponent<Browser>();
		browser.UIHandler = null;
		browser.Resize(110, 110);

		return browser;
	}
}

}
