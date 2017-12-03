using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItemsWithTable : MonoBehaviour {



    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.GetComponent<Item>())
    //    {
    //        if (other.gameObject.GetComponent<Item>().touchesTable && !other.gameObject.GetComponent<Item>().isGrabbed)
    //        {
    //            StartCoroutine(ConstrainMovement(other));
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.GetComponent<Item>())
    //    {
    //        //other.gameObject.transform.parent = null;
    //        other.gameObject.GetComponent<Item>().isGrabbed = false;
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Item>())
        {
            collision.gameObject.GetComponent<Item>().touchesTable = true;
            if (collision.gameObject.GetComponent<Item>().isGrabbed == false)
            {
                StartCoroutine(ConstrainMovement(collision));
            }
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.GetComponent<Item>().touchesTable = false;
        collision.gameObject.GetComponent<Item>().isGrabbed = false;
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.GetComponent<Item>())
    //    {
    //        if (collision.gameObject.GetComponent<Item>().touchesTable && !collision.gameObject.GetComponent<Item>().isGrabbed)
    //        {
    //            StartCoroutine(ConstrainMovement(collision));
    //        }
    //    }
    //}

    private IEnumerator ConstrainMovement(Collision other)
    {
        yield return new WaitForSeconds(.3f);
        other.gameObject.transform.parent = gameObject.transform;
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
