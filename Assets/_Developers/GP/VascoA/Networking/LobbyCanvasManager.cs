using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvasManager : MonoBehaviour
{
    
    [SerializeField] private PlayerSetupCanvas _playerSetupCanvas;
    public PlayerSetupCanvas PlayerSetupCanvas { get { return _playerSetupCanvas; } }


    [SerializeField] private LobbyCanvas _lobbyCanvas;
    public LobbyCanvas LobbyCanvas { get { return _lobbyCanvas; } }

    

    public void Initialize()
    {
        PlayerSetupCanvas.Initialize(this);
        LobbyCanvas.Initialize(this);
    }
}
