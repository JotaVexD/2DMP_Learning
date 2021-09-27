using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class NetworkManager2DMP : NetworkManager
{
    public Action OnConnected;
    public Action OnDisconnected;
    public List<GameObject> playerPrefabs;

    
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnConnected?.Invoke();

    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server Started");

    }
}

