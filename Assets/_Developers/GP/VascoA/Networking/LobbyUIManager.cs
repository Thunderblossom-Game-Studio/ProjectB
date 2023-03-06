using FishNet.Managing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    NetworkManager networkManager;

    private void Awake()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        exitButton = GameObject.Find("Exit Button").GetComponent<Button>();


        exitButton.onClick.AddListener(() => networkManager.ClientManager.StopConnection());

        exitButton.onClick.AddListener(() => SceneManager.LoadScene("MultiplayerMenu"));
    }
    
    
}
