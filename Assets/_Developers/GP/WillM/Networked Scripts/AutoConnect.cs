using FishNet.Managing;
using ParrelSync;
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

        if (!ClonesManager.IsClone())
        {
            _nm.ServerManager.StartConnection();
        }
        _nm.ClientManager.StartConnection();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    _nm.ClientManager.StopConnection();
        //}

        //if (Input.GetKeyDown(KeyCode.Minus))
        //{
        //    _nm.ServerManager.StartConnection();
        //}

        //if (Input.GetKeyDown(KeyCode.Equals))
        //{
        //    _nm.ClientManager.StartConnection();
        //}


    }
}
