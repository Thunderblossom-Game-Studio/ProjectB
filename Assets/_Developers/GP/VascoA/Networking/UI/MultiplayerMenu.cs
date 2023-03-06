using FishNet;
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiplayerMenu : MonoBehaviour
{    
    [SerializeField] private Button hostButton;
    
    [SerializeField] private Button joinButton;

    [SerializeField] private TMPro.TMP_Text connectionStatusText;

    private void Awake()
    {
        InstanceFinder.NetworkManager.ClientManager.OnClientConnectionState += OnClientConnecting;
    }

    private void Start()
    {
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

    private void OnClientConnecting(ClientConnectionStateArgs localClient)
    {
        Debug.Log("Local client connection state: " + localClient.ConnectionState);

        if (localClient.ConnectionState == LocalConnectionState.Starting)
        {
            hostButton.interactable = false;

            joinButton.interactable = false;

            connectionStatusText.color = Color.yellow;
            connectionStatusText.text = "Connecting...";
        }
 
        if (localClient.ConnectionState == LocalConnectionState.Stopped)
        {
            //hostButton.interactable = true;

            //joinButton.interactable = true;

            connectionStatusText.color = Color.red;
            connectionStatusText.text = "Connection Failed";
        }
    }
    
}
