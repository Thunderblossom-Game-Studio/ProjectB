using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{

    [Header("LobbyUI")]
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private TMPro.TMP_Text infoText;

    [Header("PlayerSetupUI")]
    [SerializeField] private GameObject playerSetupUI;


    [SerializeField] List<int> clientIDs;
    


    private void Start()
    {
        InstanceFinder.NetworkManager.ServerManager.OnRemoteConnectionState += OnRemoteClientConnecting;
        InstanceFinder.NetworkManager.ClientManager.OnClientConnectionState += OnClientConnecting;

        

        playerSetupUI.SetActive(true);

        //if (IsServer)
        //    infoText.text = "You are the host.";

        //if(IsClient)
        //    infoText.text = "You are a client.";

    }

    
    private void OnRemoteClientConnecting(NetworkConnection conn, RemoteConnectionStateArgs remote)
    {
        

        if (remote.ConnectionState == RemoteConnectionState.Started)
        {
            clientIDs.Add(remote.ConnectionId);
        }

        if (remote.ConnectionState == RemoteConnectionState.Stopped)
        {
            clientIDs.Remove(remote.ConnectionId);
        }

        
    }
    
    private void OnClientConnecting(ClientConnectionStateArgs local)
    {
        
    }


    public void OnClick_Exit()
    {
        InstanceFinder.ClientManager.StopConnection();
        
        if (IsServer)
        {
            InstanceFinder.ServerManager.StopConnection(false);
        }
    }

}
