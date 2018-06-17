using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkManager : MonoBehaviour {

    public bool isAtStartup = true;
    NetworkClient myClient;
    private NetworkManager manager;

    public InputField adress;

    private void Start()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void SetupHost()
    {
        SetupServer();
        SetupLocalClient();
        manager.StartHost();
    }

    // Create a server and listen on a port
    public void SetupServer()
    {
        NetworkServer.Listen(4444);
        isAtStartup = false;
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect(adress.text, 4444);
        isAtStartup = false;
        manager.StartClient();
    }

    // Create a local client and connect to the local server
    public void SetupLocalClient()
    {
        myClient = ClientScene.ConnectLocalServer();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        isAtStartup = false;
    }

    // client function
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }
}
