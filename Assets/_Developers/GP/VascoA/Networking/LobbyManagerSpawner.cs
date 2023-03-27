using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class LobbyManagerSpawner : MonoBehaviour
{
    /// <summary>
    /// Prefab to spawn for LobbyNetwork.
    /// </summary>
    [Tooltip("Prefab to spawn for LobbyNetwork.")]
    [SerializeField]
    private NetworkObject _lobbyNetworkPrefab;

    /// <summary>
    /// NetworkManager on this object.
    /// </summary>
    private NetworkManager _networkManager;

    

    private void Awake()
    {
        _networkManager = GetComponent<NetworkManager>();
        _networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene old, Scene newScene)
    {
        if (newScene.name != "Lobby")
            return;
        if (!_networkManager.ServerManager.OneServerStarted())
            return;

        NetworkObject nob = Instantiate(_lobbyNetworkPrefab);
        Scene scene = UnitySceneManager.GetSceneByName("Lobby");
        UnitySceneManager.MoveGameObjectToScene(nob.gameObject, scene);
        _networkManager.ServerManager.Spawn(nob.gameObject);
    }

    /// <summary>
    /// Called when the local server state changes.
    /// </summary>
    private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
    {
        if (obj.ConnectionState != LocalConnectionState.Started)
            return;
        if (!_networkManager.ServerManager.OneServerStarted())
            return;

        //NetworkObject nob = Instantiate(_lobbyNetworkPrefab);
        //Scene scene = UnitySceneManager.GetSceneByName("Lobby");
        //UnitySceneManager.MoveGameObjectToScene(nob.gameObject, scene);
        //_networkManager.ServerManager.Spawn(nob.gameObject);
    }
}
