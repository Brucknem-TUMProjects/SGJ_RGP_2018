using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncScaleRotation : NetworkBehaviour {

    //SyncScaleRotation toSync = original.GetComponent<SyncScaleRotation>();
    //toSync.rotation = original.transform.rotation;
    //toSync.scale = original.transform.localScale;

    [SyncVar]
    public Vector3 scale;
    [SyncVar]
    public Quaternion rotation;

    void Start()
    {
        transform.localScale = scale;
        transform.rotation = rotation;
    }
}
