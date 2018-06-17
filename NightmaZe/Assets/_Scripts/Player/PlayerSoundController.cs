using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour {
    public AudioSource audio;

    float timer;
    float vel;
    bool onGround;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (vel > .1f)
        {
            if (Time.time > timer && onGround)
            {
                audio.Play();
                timer = Time.time + 2 / vel;
            }
        }
	}

    public void UpdateStats(float vel, bool onGround)
    {
        this.vel = vel;
        this.onGround = onGround;
    }
}
