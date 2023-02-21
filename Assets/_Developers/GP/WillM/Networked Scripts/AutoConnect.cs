using FishNet.Managing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoConnect : MonoBehaviour
{
    private NetworkManager _nm;

    private void Start()
    {
        _nm = gameObject.GetComponent<NetworkManager>();

        if (_nm == null)
        {
            Debug.LogError("NetworkManager not found");
        }
        else
        {
            _nm.ClientManager.StartConnection();
        }
    }
}
