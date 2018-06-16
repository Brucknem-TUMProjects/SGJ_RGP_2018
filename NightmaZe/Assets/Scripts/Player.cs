using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{

	public enum Posture
	{
		standing = 0,
		crouching = 1,
		proning = 2,
	};

	[System.Serializable]
	public class MoveSettings
	{
		public float runMultiplier = 1.3f;
		public float walkVelocity = 20;
		public float crouchVelocity = 15;
		public float proneVelocity = 5;
		public float jumpVelocity = 5f;
		public float rotationSpeed = 7;
		public float distanceToGround = 1.3f;
		public LayerMask ground;
	}

	#region parameters
	public MoveSettings moveSettings;
	public Transform spawnpoint;
	public float holdTimeForProne = 1.0f;
	public Collider[] postureCollider;          //0: Standing, 1:Crouching, 2:Proning
	#endregion

	#region private attributes
	private Rigidbody playerRigid;
	private Vector3 velocity;
	private GameObject camera;

	private float downTime;
	private float speedValue;
	private Posture currentPosture;
	private bool skipButtonUpEvent, stopReadingInput;
	private Vector3 postureHeights;

	// Inputs
	bool jumpInput;

	#endregion

	// Reset attributes to default values
	public void Reset()
	{
		velocity = Vector3.zero;
		downTime = 0;
		speedValue = moveSettings.walkVelocity;
		jumpInput = skipButtonUpEvent = false;
		currentPosture = Posture.standing;
		postureCollider[0].enabled = true;
		postureCollider[1].enabled = false;
		postureCollider[2].enabled = false;
	}

	// Check if player is on ground
	bool OnGround()
	{
		return Physics.Raycast(transform.position, Vector3.down, moveSettings.distanceToGround, moveSettings.ground);
	}

	bool CheckForCeiling(float height)
	{
		return Physics.Raycast(transform.position, Vector3.up, height, 10);
	}

	// Use this for initialization
	void Start()
	{
		// ---TODO: Spawn, Reset
		playerRigid = GetComponent<Rigidbody>();
		camera = transform.GetChild(0).gameObject;
		camera.GetComponent<MouseLook>().Init(this.transform, camera.transform);
		Reset();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		//Physics.gravity = Vector3.down * 20;
	}

	// Update is called once per frame
	void Update()
	{
		camera.GetComponent<MouseLook>().LookRotation(this.transform, camera.transform);
		//CheckLife();
		//GetTimer();
		//if (!ableToMove)
		//    return;
		GetJumpInput();
		GetButtonInput();
	}

	void FixedUpdate()
	{
		Move();
		if (jumpInput)
			Jump();
	}

	void GetJumpInput()
	{
		if (Input.GetButtonDown("Jump"))
			jumpInput = true;
	}

	void Move()
	{
		velocity = new Vector3(Input.GetAxis("Horizontal"), playerRigid.velocity.y, Input.GetAxis("Vertical"));
		velocity *= speedValue;
		if (Input.GetAxis("RT") > 0)
		{
			if(velocity.z > 0)
				velocity.z *= moveSettings.runMultiplier;
		}
		// Default
		velocity.y = playerRigid.velocity.y;
		playerRigid.velocity = Vector3.Lerp(playerRigid.velocity, transform.TransformDirection(velocity), Time.fixedDeltaTime * 7f);
	}


	void GetButtonInput()
	{
		// Crouching, Proning = Hold Crouch Button	
		if (Input.GetButton("ControllerXButton"))
		{
			if (!stopReadingInput)
			{
				downTime += Time.deltaTime;
				skipButtonUpEvent = false;
			}
			else
				skipButtonUpEvent = true;
		}
		if (Input.GetButtonUp("ControllerXButton") || downTime >= holdTimeForProne)
		{
			if (downTime < holdTimeForProne && !skipButtonUpEvent)
			{
				// User pressing Crouch Button for a shorter time then needed for prone
				// Possible: Player is either standing (-> crouch), crouching (-> stand up), proning(-> crouching)
				if (currentPosture == Posture.standing)
				{
					Crouch();
				}
				else if (currentPosture == Posture.crouching)
				{
					StandUp();
				}
				else if (currentPosture == Posture.proning)
				{
					Crouch();
				}
			}
			else if (!stopReadingInput)
			{
				// User holds Crouch Button long enough for prone
				//Possible: Player either standing(->prone), crouching(->prone), proning(->standup)
				downTime = 0;
				skipButtonUpEvent = true;
				stopReadingInput = true;
				if (currentPosture == Posture.standing)
				{
					Prone();
				}
				else if (currentPosture == Posture.crouching)
				{
					Prone();
				}
				else if (currentPosture == Posture.proning)
				{
					StandUp();
				}
			}

			if (Input.GetButtonUp("ControllerXButton"))
			{
				stopReadingInput = false;
				skipButtonUpEvent = false;
			}
			downTime = 0;
		}
	}



	bool StandUp()
	{
		// Check if standing up is possible
		bool canStandUp = true;
		if (currentPosture == Posture.crouching)
		{
			canStandUp = !CheckForCeiling(0.9f);
		}
		else if (currentPosture == Posture.proning)
		{
			canStandUp = !CheckForCeiling(1.3f);
		}

		// Perform standing up
		if (!canStandUp)
		{
			Debug.Log("Cannot stand up, ceiling too low! Du hasch koin blatz nedda, bisch zu fett");
			return false;
		}

		// TODO: Switch Colliders, animate Character 
		postureCollider[0].enabled = true;
		postureCollider[1].enabled = false;
		postureCollider[2].enabled = false;
		Debug.Log("Stood up");
		currentPosture = Posture.standing;
		speedValue = moveSettings.walkVelocity;
		return true;
	}

	bool Crouch()
	{
		if (currentPosture == Posture.proning)
		{
			if (CheckForCeiling(0.1f))
			{
				// TODO: Switch Colliders, animate Character
				Debug.Log("Cannot go to crouch from prone!");
				return false;
			}
		}

		postureCollider[0].enabled = false;
		postureCollider[1].enabled = true;
		postureCollider[2].enabled = false;
		currentPosture = Posture.crouching;
		speedValue = moveSettings.crouchVelocity;
		Debug.Log("Going into Crouch");
		return true;
	}

	bool Prone()
	{
		// TODO: Switch Collider, animate Character to prone
		postureCollider[0].enabled = false;
		postureCollider[1].enabled = false;
		postureCollider[2].enabled = true;
		currentPosture = Posture.proning;
		speedValue = moveSettings.proneVelocity;
		// Nice to have: Check if proning is possible! (?)
		return true;
	}
    
	// FixedUpdate/Physics methods
	void Jump()
	{
		if (OnGround() && currentPosture == Posture.standing)
		{
			playerRigid.velocity = new Vector3(playerRigid.velocity.x, moveSettings.jumpVelocity, playerRigid.velocity.z);
		}
		jumpInput = false;
	}

	// Collision/Trigger methods
	void OnTriggerEnter(Collider col)
	{
		switch (col.tag)
		{
			#region trigger
			case "DeathZone":
				Debug.Log("Player Dieded");
				break;
			case "Respawn":
				spawnpoint = col.transform;
				break;
				#endregion
		}
	}
	void OnTriggerStay(Collider col)
	{

	}

	void OnTriggerExit(Collider col)
	{

	}

	// public methods
	public void Spawn()
	{
		transform.position = spawnpoint.position;
	}

	public void Death()
	{
		// ---TODO: Action on death
		StartCoroutine(OnDeath());
	}

	IEnumerator OnDeath()
	{
		//ableToMove = false;
		//yield return new WaitForSeconds(3f);
		//ui.SetAlpha(1);
		//Travel.SetDestination("GameOver");
		yield return null;
	}
}
