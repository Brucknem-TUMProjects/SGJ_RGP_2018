using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Dreamer : NetworkBehaviour
{
    [SyncVar(hook = "OnKeyCollected")]
	bool keyCollected = false;
    [SyncVar(hook = "OnLanternCollected")]
	bool lanternCollected = false;

    public PlayerAnimator anim;
	public Transform keyPocket = null;
	public Transform lanternPocket = null;

    void OnKeyCollected(bool keyCollected)
    {
        //TODO Sound etc..
    }

    void OnLanternCollected(bool lanternCollected)
    {
        //TODO adjust mesh
    }


    void Update()
	{
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetButtonDown("InteractLeft"))
        {
            if (lanternCollected)
            {
                anim.ArmsForward(true);
            }
        }
        else
            anim.ArmsForward(false);
		if (Input.GetButtonDown("ControllerYButton"))
		{
			if (lanternCollected)
			{
				//lanternPocket.GetChild(0).SetParent(null);
				//Debug.Log("Laterne weggeworfen");
				//lanternCollected = false;
			}
		}

	}


	void OnTriggerStay(Collider col)
	{
		if (Input.GetButtonDown("ControllerYButton"))
		{
            if (col.tag == "Key" || col.tag == "Lantern")
            {
                // TODO: Interact with thing?
                if (col.tag == "Key")
                {
                    if (keyCollected)
                    {
                        Debug.Log("Cannot carry more than 1 key");
                        return;
                    }
                    keyCollected = true;
                    col.transform.SetParent(keyPocket);

                    // TODO: Display in GUI that key is now collected (?)
                }
                else if (col.tag == "Lantern")
                {
                    if (lanternCollected)
                    {
                        Debug.Log("Cannot carry more than 1 lantern");
                        return;
                    }
                    StartCoroutine(CollectLantern());
                    col.transform.SetParent(lanternPocket);

                    // Put gamobjects as child to player (--> fixed at tighs/hand etc.)
                }
                col.transform.localPosition = Vector3.zero;
                col.transform.localRotation = Quaternion.identity;
                col.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
		if (col.tag == "Door")
		{
			if (Input.GetButtonDown("InteractionRight") && keyCollected)
			{
				Debug.Log("Key --> Doorlock . . .");
				// TODO: Call script on door and [increment keyCount] for unlocking progress
			}
		}

		if (col.tag == "Lightzone")
		{
			// TODO: Whatever a lightzone does..
		}
	}

	IEnumerator CollectLantern()
	{
		yield return new WaitForEndOfFrame();
		lanternCollected = true;
        anim.HandsClosed(true);
	}
}
