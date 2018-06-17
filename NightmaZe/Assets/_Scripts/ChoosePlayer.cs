using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChoosePlayer : MonoBehaviour {

    public Dropdown selector;

    public GameObject[] prefabs;

	// Use this for initialization
	void Start () {
        selector.onValueChanged.AddListener(delegate { Listener(selector); });
	}

    void Listener(Dropdown dropdown)
    {
        Debug.Log("listener");
        GetComponent<NetworkLobbyManager>().gamePlayerPrefab = prefabs[dropdown.value];
    }
}
