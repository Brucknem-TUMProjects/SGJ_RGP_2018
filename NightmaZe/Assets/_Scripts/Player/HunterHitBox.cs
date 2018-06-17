using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterHitBox : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Dreamer")
        {
            // TODO: decrease health of other
        }
    }
}
