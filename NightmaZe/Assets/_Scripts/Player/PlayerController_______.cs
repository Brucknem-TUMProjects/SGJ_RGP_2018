using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerController_______ : NetworkBehaviour {

    //Holds player attributes
    private class Status
    {
        public int Health { get; set; }
        public int Stamina { get; set; }
        public int Panic { get; set; }

        public Status() : this(100, 100, 0) {}

        public Status(int health, int stamina, int panic)
        {
            Health = health;
            Stamina = stamina;
            Panic = panic;
        }
    }

    private class Inventory
    {
        public List<Key> keys;

        public Inventory()
        {
            keys = new List<Key>();
        }
    }

    private Status status;
    private Inventory inventory;

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            Init();

            GetComponent<PlayerMovement>().enabled = true;
            Camera.main.transform.parent = transform;
            Camera.main.transform.position = transform.position;
            Camera.main.transform.rotation = Quaternion.identity;// (transform.forward);
            Transform t = Camera.main.transform;
            Transform me = transform;
        }
    }

    //Sets fields on start
    void Init()
    {
        status = new Status();
        inventory = new Inventory();
    }

    //Adds keys to inventory
    public void AddKey(Key key)
    {
        inventory.keys.Add(key);
    }

    //Gets list of keys
    public List<Key> GetKeys() {
        return inventory.keys;
    }

    private void Update()
    {
        //GetComponentInChildren<TextMesh>().text = pname;
        //Camera.main.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.y, 0);
        //Camera.main.transform.rotation = Quaternion.Slerp(Quaternion.Euler(new Vector3(0, Vector3.Angle(Camera.main.transform.forward, transform.forward), 0)), Camera.main.transform.rotation, 0.2f);
        //Camera.main.transform.Rotate(new Vector3(0, Vector3.Angle(Camera.main.transform.forward, transform.forward), 0));
    }

    //[SyncVar]
    //public string pname = "Player";

    private void OnGUI()
    {
        //if (isLocalPlayer)
        //{
        //    pname = GUI.TextField(new Rect(25, Screen.height - 40, 100, 30), pname);
        //    if(GUI.Button(new Rect(130, Screen.height - 40 , 80, 30), "Change"))
        //    {
        //        CmdChangeName(pname);
        //    }
        //}
    }

    [Command]
    public void CmdChangeName(string name)
    {
        //Debug.Log("Changed name");
        //pname = name;
    }

   
}
