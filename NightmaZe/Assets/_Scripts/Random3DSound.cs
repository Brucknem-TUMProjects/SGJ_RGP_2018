using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Random3DSound : MonoBehaviour {

    /*
     * Play a random audioclip at a random x/z postion around an object(listener)
    */
    #region Vars
    public GameObject listener;
    public AudioClip[] sounds;
    private AudioSource audioSource;
    public float minDistance;               // closest distance an audioclip can be played around the listener
    public float maxDistance;               // farest distance an audioclip can be played around the listener
    public float minTimeToWait;             // minimum time between to audioclips
    public float maxTimeToWait;             // maximum time between to audioclips
    public float chancetoPlay;              // probability of playing a audioclip

    private float timeToWait;               // time stemp when the next could be played
    private float randomFloat;              // var to set a random float var
    #endregion

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        randomFloat = Random.Range(minTimeToWait, maxTimeToWait);
        timeToWait = Time.time + randomFloat;
		listener = GetComponentInParent<GameObject>();
		transform.SetParent(null);
    }
	
	// Update is called once per frame
	void Update () {
        if(Time.time >= timeToWait)
        {
            randomFloat = Random.Range(minTimeToWait, maxTimeToWait);
            timeToWait = Time.time + randomFloat;
            randomFloat = Random.Range(0f,1f);
            if(randomFloat <= chancetoPlay)
            {
                randomFloat = Random.Range(0f, 1f);
                Vector3 circlePoint = new Vector3(Mathf.Cos(randomFloat), 0, Mathf.Sin(randomFloat)) * Random.Range(minDistance, maxDistance);
                //Debug.Log(listener.transform.position + " + " +circlePoint);
                this.transform.position = listener.transform.position + circlePoint;
                //Debug.Log(this.transform.position);
                int randomInt;
                randomInt = Random.Range(0, sounds.Length);
                audioSource.clip = sounds[randomInt];
                audioSource.Play();
                timeToWait += sounds[randomInt].length;
            }
        }
	}
}
