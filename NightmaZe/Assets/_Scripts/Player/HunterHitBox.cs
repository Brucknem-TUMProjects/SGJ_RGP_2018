using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterHitBox : MonoBehaviour {
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Dreamer")
        {
            col.transform.GetChild(0).GetComponent<PlayerAnimator>().Kill();
        }
    }
}
