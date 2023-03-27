using FishNet;
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MultiplayerMenuUIManager : MonoBehaviour
{
    [Header("Networking")]
    [SerializeField] private NetworkManager networkManager;
    private bool isConnecting;

    [Header("UI")]
    [SerializeField] private Button hostButton;  
    [SerializeField] private Button joinButton;
    [SerializeField] private TMPro.TMP_Text connectionStatusText;

    private void Start()
    {
        networkManager = InstanceFinder.NetworkManager;

        networkManager.ClientManager.OnClientConnectionState += OnClientConnecting;

        hostButton.onClick.AddListener(() =>
        {       
            InstanceFinder.ServerManager.StartConnection();

            InstanceFinder.ClientManager.StartConnection();
          
        });

        joinButton.onClick.AddListener(() =>
        {
            InstanceFinder.ClientManager.StartConnection();          
        });

    }

    private void Update()
    {
        ButtonInputLock(isConnecting);
    }

    private void OnDisable()
    {
        networkManager.ClientManager.OnClientConnectionState -= OnClientConnecting;
    }

    private void ButtonInputLock(bool state)
    { 
        if (state)
        {
            hostButton.interactable = false;
            
            joinButton.interactable = false;

            SetConnectionText("Connecting...", Color.yellow);
        }
        else
        {
            hostButton.interactable = true;
            
            joinButton.interactable = true;

            SetConnectionText("Connection Stopped", Color.red);
        }
    }

    private void SetConnectionText(string text, Color color)
    {
        connectionStatusText.color = color;
        connectionStatusText.text = text;
    }
    
    private void OnClientConnecting(ClientConnectionStateArgs localClient)
    {
        if (networkManager == null) return;
        
        if (localClient.ConnectionState == LocalConnectionState.Starting)
        {
            isConnecting = true;          
        }

        if (localClient.ConnectionState == LocalConnectionState.Stopped)
        {
            isConnecting = false;
        }
    }

}
