using FishNet;
using FishNet.Connection;
using FishNet.Object;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    private static LobbyManager Instance;
    
    protected LobbyCanvasManager LobbyCanvasManager;


    public Dictionary<NetworkConnection, string> LoggedInUsernames = new Dictionary<NetworkConnection, string>();

    public event Action<NetworkObject> OnClientLoggedIn;

    

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        Instance = this;

        LobbyCanvasManager = GameObject.FindObjectOfType<LobbyCanvasManager>();
        if (LobbyCanvasManager == null)
        {
            Debug.LogError("LobbyCanvasManager not found.");
            return;
        }
        LobbyCanvasManager.Initialize();

        
        InstanceFinder.SceneManager.OnClientLoadedStartScenes += OnClientLoadedStartScenes;
    }

    #region Subscribed Events

    private void OnClientLoadedStartScenes(NetworkConnection conn, bool asServer)
    {

        
    }
    
    #endregion




    public static void SignIn(string username)
    {
        Instance.InternalSignIn(username);
    }    
    /// <summary>
    /// Called when a client has signed in.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="sender"></param>
    [ServerRpc(RequireOwnership = false)]
    private void InternalSignIn(string username, NetworkConnection sender = null)
    {
        ClientInfo ci;
        if (!FindClientInstance(sender, out ci))
            return;

        LoggedInUsernames[ci.Owner] = username;

        ci.SetUsername(username);
        OnClientLoggedIn?.Invoke(ci.NetworkObject);
        TargetSignInSuccess(sender, username);

    }

    [TargetRpc]
    private void TargetSignInSuccess(NetworkConnection conn, string username)
    {
        LobbyCanvasManager.PlayerSetupCanvas.gameObject.SetActive(false);
        LobbyCanvasManager.LobbyCanvas.gameObject.SetActive(true);
        LobbyCanvasManager.LobbyCanvas.AddPlayerCard(username);
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
