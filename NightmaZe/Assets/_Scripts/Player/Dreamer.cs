using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dreamer : MonoBehaviour
{
	bool keyCollected = false;
	bool lanternCollected = false;

	public Transform keyPocket = null;
	public Transform lanternPocket = null;

	void Update()
	{
		if (Input.GetButtonDown("InteractLeft"))
		{
			if (lanternCollected)
			{
				// TODO: Play lantern anim and emit light dass kracht!
			}
		}
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
				col.transform.localPosition = Vector3.zero;
				col.transform.localRotation = Quaternion.identity;
				// TODO: Display in GUI that key is now collected (?)
			}
			 if (col.tag == "Lantern")
			{
				if (lanternCollected)
				{
					Debug.Log("Cannot carry more than 1 lantern");
					return;
				}
				StartCoroutine(CollectLantern());
				col.transform.SetParent(lanternPocket);
				col.transform.localPosition = Vector3.zero;
				col.transform.localRotation = Quaternion.identity;
				// Put gamobjects as child to player (--> fixed at tighs/hand etc.)
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
	}
}
