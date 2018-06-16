using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dreamer : MonoBehaviour
{
	bool keyCollected = false;
	bool lanternCollected = false;

	public GameObject key = null;
	public GameObject lantern = null;

	void OnTriggerStay(Collider col)
	{
		if (Input.GetButtonDown("Interact"))
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
				key = col.gameObject;
				// TODO: Display in GUI that key is now collected (?)
			}
			if (col.tag == "Lantern")
			{
				if (lanternCollected)
				{
					Debug.Log("Cannot carry more than 1 lantern");
					return;
				}
				lanternCollected = true;
				lantern = col.gameObject;
				// Put gamobjects as child to player (--> fixed at tighs/hand etc.)
			}
			if (col.tag == "Door")
			{
				if (keyCollected)
				{
					Debug.Log("Key --> Doorlock . . .");
					// TODO: Call script on door and [increment keyCount] for unlocking progress
				}
			}
		}
		if (col.tag == "Lightzone")
		{
			// TODO: Whatever a lightzone does..
		}
	}
}
