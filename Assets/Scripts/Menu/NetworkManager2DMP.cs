using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public enum NetworkState {Offline, Handshake, Lobby, World}

[RequireComponent(typeof(Database))]
public class NetworkManager2DMP : NetworkManager
{
    public NetworkState state = NetworkState.Offline;
    public Action OnConnected;
    public Action OnDisconnected;
    public List<GameObject> playerPrefabs;


    public Dictionary<NetworkConnection, string> lobby = new Dictionary<NetworkConnection, string>();
    

    
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnConnected?.Invoke();

    }

    public override void OnStartServer()
    {
        Database.singleton.Connect();

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

