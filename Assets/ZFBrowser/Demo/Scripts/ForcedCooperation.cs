using System.Collections;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {

/** Forces whoWillComply to be behind our z when Comply() is called. */
public class ForcedCooperation : MonoBehaviour {
	public Transform whoWillComply;
	public float howLongWillTheyComply;

	public void Comply() {
		StartCoroutine(_Comply());
	}

	protected IEnumerator _Comply() {
		var t0 = Time.time;

		do {
			var pos = transform.InverseTransformPoint(whoWillComply.position);

			if (pos.z > 0) {
				pos.z = 0;
				whoWillComply.position = transform.TransformPoint(pos);
			}

			yield return null;
		} while (Time.time - t0 < howLongWillTheyComply);

	}


}
}
