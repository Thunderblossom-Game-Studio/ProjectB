using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateJoinCurrentRoomCanvas : MonoBehaviour
{
    public LobbyCanvasManager LobbyCanvasManager { get; private set; }

    [SerializeField] private CreateRoomMenu _createRoomMenu;

    [SerializeField] private JoinRoomMenu _joinRoomMenu;
    public JoinRoomMenu JoinRoomMenu { get { return _joinRoomMenu; } }

    [SerializeField] private CurrentRoomMenu _currentRoomMenu;
    public CurrentRoomMenu CurrentRoomMenu { get { return _currentRoomMenu; } }


    private CanvasGroup _canvasGroup;

    
    public void Initialize(LobbyCanvasManager lcm)
    {
        LobbyCanvasManager = lcm;
        _canvasGroup = GetComponent<CanvasGroup>();
        _createRoomMenu.Initialize();
        _joinRoomMenu.Initialize();
        _currentRoomMenu.Initialize();
        Reset();
    }

    public void Reset()
    {
        SignInFailed();
        _createRoomMenu.Reset();
        JoinRoomMenu.Reset();
        CurrentRoomMenu.Reset();
    }

    public void SignInSuccess()
    {
        _canvasGroup.SetActive(true, true);
    }
    
    public void SignInFailed()
    {
        _canvasGroup.SetActive(false, true);
    }

    public void ShowRoomCreatedSuccess(RoomDetails roomDetails)
    {
        JoinRoomMenu.ShowRoomCreatedSuccess();
        _createRoomMenu.ShowRoomCreatedSuccess();
        CurrentRoomMenu.ShowRoomCreatedSuccess(roomDetails);
    }
   
    public void ShowRoomCreatedFailed(string failedReason)
    {
        _createRoomMenu.ShowRoomCreatedFailed();
        CurrentRoomMenu.ShowRoomCreatedFailed(failedReason);
    }


    public void ShowRoomJoinedSuccess(RoomDetails roomDetails)
    {
        JoinRoomMenu.ShowRoomJoinedSuccess();
        _createRoomMenu.ShowRoomJoinedSuccess();
        CurrentRoomMenu.ShowRoomJoinedSuccess(roomDetails);
    }

    public void ShowRoomJoinedFailed(string failedReason)
    {
        JoinRoomMenu.ShowRoomJoinedFailed();
        _createRoomMenu.ShowRoomJoinedFailed();
        CurrentRoomMenu.ShowRoomJoinedFailed(failedReason);
    }

    public void ShowRoomLeftSuccess()
    {
        JoinRoomMenu.ShowRoomLeftSuccess();
        _createRoomMenu.ShowRoomLeftSuccess();
        CurrentRoomMenu.ShowRoomLeftSuccess();
    }

    public void ShowRoomLeftFailed()
    {
        JoinRoomMenu.ShowRoomLeftFailed();
        _createRoomMenu.ShowRoomLeftFailed();
        CurrentRoomMenu.ShowRoomLeftFailed();
    }

    public void ShowStartGame(bool success, RoomDetails roomDetails, string failedReason)
    {
        /* StartGame response won't affect
         * JoinRoonMenu or CreateRoomMenu as they
         * both are already hidden because of being 
         * in a CurrentRoom. Only current room must update. */
        CurrentRoomMenu.ShowStartGame(success, roomDetails, failedReason);
    }
}
