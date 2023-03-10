using FishNet;
using FishNet.Managing;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : NetworkBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private TMPro.TMP_Text infoText;

    private void Start()
    {
        exitButton.onClick.AddListener(() => InstanceFinder.ClientManager.StopConnection());
        
        if (IsServer)
        {
            exitButton.onClick.AddListener(() => InstanceFinder.ServerManager.StopConnection(false));

            infoText.text = "You are the host.";
        }
        else
        {
            infoText.text = "You are a client. ID: " + InstanceFinder.ClientManager.GetInstanceID();
        }

    }

}
