using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;
using FishNet;

public class ClientInfo : NetworkBehaviour
{

    public static ClientInfo Instance;

    [SyncVar][SerializeField] private string username;
    
    public void SetUsername(string name)
    {
        username = name;
    }

    public string GetUsername() 
    {
        return username;
    }

    public static ClientInfo ReturnClientInstance(NetworkConnection conn)
    {
        if (InstanceFinder.IsServer && conn != null)
        {
            NetworkObject nob = conn.FirstObject;
            return (nob == null) ? null : nob.GetComponent<ClientInfo>();
        }
        else
        {
            return Instance;
        }
    }

}

