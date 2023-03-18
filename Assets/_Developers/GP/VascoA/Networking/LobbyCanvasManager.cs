using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvasManager : MonoBehaviour
{
    
    [SerializeField] private PlayerSignUpCanvas _playerSignUpCanvas;
    public PlayerSignUpCanvas PlayerSignUpCanvas { get { return _playerSignUpCanvas; } }


    [SerializeField] private LobbyCanvas _lobbyCanvas;
    public LobbyCanvas LobbyCanvas { get { return _lobbyCanvas; } }

    

    public void Initialize()
    {
        PlayerSignUpCanvas.Initialize(this);
        LobbyCanvas.Initialize(this);
    }
}
