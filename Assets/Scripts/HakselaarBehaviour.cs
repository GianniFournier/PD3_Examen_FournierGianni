using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HakselaarBehaviour : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Log")
        {
            Destroy(other.gameObject);
            Debug.Log("[HAKSELAAR] Log Destroyed");
        }
    }

}
