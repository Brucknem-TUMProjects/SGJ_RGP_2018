using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkManager : MonoBehaviour {

    private static NetworkManager manager;
    private static NetworkClient client;

    public void Start()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void CreateHost()
    {
        client = manager.StartHost();
    }

    private void CreateClient()
    {
        if(client != null)
        {
            manager.StartClient();
        }
    }
}
