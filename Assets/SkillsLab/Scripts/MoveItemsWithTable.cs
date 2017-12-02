using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItemsWithTable : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Item>())
        {
            if (other.gameObject.GetComponent<Item>().touchesTable && !other.gameObject.GetComponent<Item>().isGrabbed)
            {
                StartCoroutine(ConstrainMovement(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            //other.gameObject.transform.parent = null;
            other.gameObject.GetComponent<Item>().isGrabbed = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Item>().touchesTable = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.GetComponent<Item>().touchesTable = false;
    }

    private IEnumerator ConstrainMovement(Collider other)
    {
        yield return new WaitForSeconds(.25f);
        other.gameObject.transform.parent = gameObject.transform;
        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
