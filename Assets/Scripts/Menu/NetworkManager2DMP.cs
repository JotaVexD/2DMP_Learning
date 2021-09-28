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

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        // instantiating a "Player" prefab gives it the name "Player(clone)"
        // => appending the connectionId is WAY more useful for debugging!
        player.name = UserConfig.Instance.charData.username;
        NetworkServer.AddPlayerForConnection(conn, player);
    }

}

