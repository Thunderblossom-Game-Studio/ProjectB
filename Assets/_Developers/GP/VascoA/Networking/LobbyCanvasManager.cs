using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvasManager : MonoBehaviour
{
    
    [SerializeField] private PlayerSignInCanvas _playerSignInCanvas;
    public PlayerSignInCanvas PlayerSignInCanvas { get { return _playerSignInCanvas; } }


    [SerializeField] private CreateJoinCurrentRoomCanvas _createJoinCurrentRoomCanvas;
    public CreateJoinCurrentRoomCanvas CreateJoinCurrentRoomCanvas { get { return _createJoinCurrentRoomCanvas; } }

    [Tooltip("Camera for the lobby.")]
    [SerializeField] private Camera _lobbyCamera = null;


    public void Initialize()
    {
        PlayerSignInCanvas.Initialize(this);
        CreateJoinCurrentRoomCanvas.Initialize(this);
    }

    public void Reset()
    {
        PlayerSignInCanvas.Reset();
        CreateJoinCurrentRoomCanvas.Reset();
    }

    public void SetLobbyCameraActive(bool active)
    {
        if (_lobbyCamera != null)
            _lobbyCamera.gameObject.SetActive(active);
    }
}
