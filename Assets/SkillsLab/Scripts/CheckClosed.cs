using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckClosed : MonoBehaviour {
    const float WAITTIMELOCK = 30f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }

    public IEnumerator AutoLock(Drawer drawer)
    {
        yield return new WaitForSeconds(WAITTIMELOCK);

    }
}
