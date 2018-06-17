using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Hunter : NetworkBehaviour
{
	[SerializeField]
	PlayerAnimator anim;
	[SyncVar]
	float cooldown = 10.0f;
	[SyncVar]
	float timer;

	void Start()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		timer = Time.time - cooldown;
	}

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		if (timer + cooldown < Time.time)
		{
			Debug.Log("Timer ready");
			if (Input.GetAxisRaw("VerticalSteuerkreuz") < 0)
			{
				// Steuerkreuz nach unten
			}
			else if (Input.GetAxisRaw("VerticalSteuerkreuz") > 0)
			{
				// Steuerkreuz nach oben --> Boost speed for 5 secs
				Debug.Log("Move BoosT");
				StartCoroutine(BoostSpeed());
			}
			else if (Input.GetAxisRaw("HorizontalSteuerkreuz") > 0)
			{
				// Steuerkreuz nach links
			}
			else if (Input.GetAxisRaw("HorizontalSteuerkreuz") < 0)
			{
				// Steuerkreuz nach rechts
			}
			else
			{
				return;
			}
			timer = Time.time;
		}
		if (Input.GetButtonDown("InteractRight"))
		{
			Attack();
		}
	}

	void Attack()
	{
		if (!anim.attacking)
			anim.Punch();
	}

	IEnumerator BoostSpeed()
	{
		GetComponent<Player>().speedBoost = 2;
		yield return new WaitForSeconds(5);
		GetComponent<Player>().speedBoost = 1;
	}
}
