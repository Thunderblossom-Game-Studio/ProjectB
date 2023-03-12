using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class ClientInfo : NetworkBehaviour
{ 
    [SyncVar]
    [SerializeField ] private string username;
    
    [ServerRpc]
    public void SetUsername(string name, ClientInfo user)
    {
        user.username = name;
    }

    public string GetUsername() 
    {
        return username;
    }
       
}

