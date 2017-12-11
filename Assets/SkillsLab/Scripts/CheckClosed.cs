using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckClosed : MonoBehaviour {
    const float WAITTIMELOCK = 30f;
    protected List<Drawer> drawersToLock;

    // Use this for initialization
    void Start()
    {
        drawersToLock = new List<Drawer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.gameObject.GetComponent<Drawer>())
        {
            if (other.gameObject.GetComponent<Drawer>().IsGrabbable && !drawersToLock.Contains(other.gameObject.GetComponent<Drawer>()))
            {
                drawersToLock.Add(gameObject.GetComponent<Drawer>());
                StartCoroutine(AutoLock(other.gameObject.GetComponent<Drawer>()));
            }
        }
    }

    public IEnumerator AutoLock(Drawer drawer)
    {
        yield return new WaitForSeconds(WAITTIMELOCK);
        drawer.SetGrabStatus(false);
        drawer.SetRigidbodyStatus(false);
        drawer.gameObject.GetComponent<CheckDrawerEmpty>().enabled = true;
        drawersToLock.Remove(drawer);
    }
}
