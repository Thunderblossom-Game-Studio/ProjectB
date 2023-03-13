using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{

    [Header("LobbyUI")]
    [SerializeField] private LobbyUI lobbyUI;
    [SerializeField] private TMPro.TMP_Text infoText;

    [Header("PlayerSetupUI")]
    [SerializeField] private GameObject playerSetupUI;


    public Dictionary<NetworkConnection, string> LoggedInUsernames = new Dictionary<NetworkConnection, string>();

    public event Action<NetworkObject> OnClientLoggedIn;

    

    private void Start()
    {      
        playerSetupUI.SetActive(true);

        //if (IsServer)
        //    infoText.text = "You are the host.";

        //if(IsClient)
        //    infoText.text = "You are a client.";

    }

    
   

    public void OnClick_Exit()
    {
        InstanceFinder.ClientManager.StopConnection();
        
        if (IsServer)
        {
            InstanceFinder.ServerManager.StopConnection(false);
        }
    }


    /// <summary>
    /// Called when a client has signed in.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="sender"></param>
    [ServerRpc(RequireOwnership = false)]
    public void SignIn(string username, NetworkConnection sender = null)
    {
        ClientInfo ci;
        if (!FindClientInstance(sender, out ci))
            return;

        LoggedInUsernames[ci.Owner] = username;

        ci.SetUsername(username);
        OnClientLoggedIn?.Invoke(ci.NetworkObject);
        playerSetupUI.SetActive(false); // Disables client sign in card
        lobbyUI.Show(); // Shows lobby UI
        lobbyUI.AddPlayerCard(username); // Adds player card to lobby UI
    }

        
    private bool FindClientInstance(NetworkConnection conn, out ClientInfo ci)
    {
        ci = null;
        if (conn == null)
        {
            Debug.LogError("Connection is null.");
            return false;
        }

        ci = ClientInfo.ReturnClientInstance(conn);
        if (ci == null)
        {
            Debug.LogError("ClientInfo not found on connection.");
            return false;
        }

        return true;


    }
}
