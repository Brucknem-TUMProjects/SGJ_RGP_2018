using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CenterNetworkManager : MonoBehaviour
{
    NetworkManagerHUD hud;

    // Use this for initialization
    void Start()
    {
        hud = GetComponent<NetworkManagerHUD>();
    }

    private void Update()
    {
        hud.offsetX = Screen.width / 2 - 110;
        hud.offsetY = Screen.height / 2 - 90;
    }
}
