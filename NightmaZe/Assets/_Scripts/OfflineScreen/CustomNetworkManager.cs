using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkManager : MonoBehaviour {

    private NetworkManager manager;
    private NetworkClient client;
    public InputField adress;

    public void Start()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void CreateHost()
    {
        manager.networkAddress = adress.text;
        client = manager.StartHost();
    }

    public void CreateClient()
    {
        
            Debug.Log("Connecting");
            manager.networkAddress = adress.text;
            manager.StartClient();
    }
}
