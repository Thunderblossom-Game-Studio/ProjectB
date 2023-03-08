using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    public NetworkManager networkManager;

    public List<int> clientIds;

    private void Awake()
    {
        networkManager = FindObjectOfType<NetworkManager>();

        networkManager.ClientManager.OnRemoteConnectionState += OnClientConnected;

        
    }

    private void OnClientConnected(RemoteConnectionStateArgs remoteClient)
    {


        if (remoteClient.ConnectionState == RemoteConnectionState.Started)
        {
            Debug.Log("Client with ID: " + remoteClient.ConnectionId + " has started");
        }

        if (remoteClient.ConnectionState == RemoteConnectionState.Stopped)
        {
            Debug.Log("Client with ID: " + remoteClient.ConnectionId + " has stopped");
        }
    }

}