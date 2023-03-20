using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvasManager : MonoBehaviour
{
    
    [SerializeField] private PlayerSignInCanvas _playerSignInCanvas;
    public PlayerSignInCanvas PlayerSignInCanvas { get { return _playerSignInCanvas; } }


    

    

    public void Initialize()
    {
        PlayerSignInCanvas.Initialize(this);
    }
}
