using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDrawerEndPos : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Drawer>())
        {
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
