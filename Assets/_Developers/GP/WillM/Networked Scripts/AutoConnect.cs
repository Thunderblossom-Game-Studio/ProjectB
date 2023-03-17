using FishNet.Managing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using ParrelSync;
#endif

public class AutoConnect : MonoBehaviour
{
#if UNITY_EDITOR
    private NetworkManager _nm;

    private void Start()
    {
        _nm = gameObject.GetComponent<NetworkManager>();

        if (_nm == null)
        {
            Debug.LogError("NetworkManager not found");
        }

        if (!ClonesManager.IsClone())
        {
            _nm.ServerManager.StartConnection();
        }
        _nm.ClientManager.StartConnection();
    }

#else
    private void Start()
    {
        NetworkManager nm = gameObject.GetComponent<NetworkManager>();
        nm.ServerManager.StartConnection();
        nm.ClientManager.StartConnection();
    }
#endif
}
