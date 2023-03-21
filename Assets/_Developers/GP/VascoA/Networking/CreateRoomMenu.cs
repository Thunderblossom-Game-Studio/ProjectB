using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomMenu : MonoBehaviour
{
    [SerializeField] private Button _createRoomButton;
    
    [SerializeField] private TMP_InputField _nameText;

    [SerializeField] private TMP_InputField _passwordText;

    [SerializeField] private TMP_Dropdown _playerCount;


    private CanvasGroup _canvasGroup;

    private bool _awaitingCreateResponse = false;
    
    public void Initialize()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _playerCount.ClearOptions();
        List<string> options = new List<string>();
        int minimum = LobbyManager.ReturnMinimumPlayers();
        int maximum = LobbyManager.ReturnMaximumPlayers();
        for (int i = minimum; i <= maximum; i++)
            options.Add(i.ToString());

        _playerCount.AddOptions(options);
        
    }

    public void Reset()
    {
        _awaitingCreateResponse = false;
        ShowRoomCreatedFailed();
    }

    public void OnClick_CreateRoom()
    {
        if (_awaitingCreateResponse)
            return;

        string roomName = _nameText.text.Trim();
        string password = _passwordText.text;
        int playerCount = ConvertPlayerCount(_playerCount.captionText.text);

        string failedReason = string.Empty;

        if (!LobbyManager.SanitizeRoomName(ref roomName, ref failedReason) || !LobbyManager.SanitizePlayerCount(playerCount, ref failedReason))
        {
            Debug.LogError(failedReason);
            //TODO VASCO
            //GlobalManager.CanvasesManager.MessagesCanvas.InfoMessages.ShowTimedMessage(failedReason, MessagesCanvas.BRIGHT_ORANGE);
        }
        else
        {
            _awaitingCreateResponse = true;
            LobbyManager.SanitizeTextMeshProString(ref password);
            LobbyManager.CreateRoom(roomName, password, playerCount);
        }
    }

    private int ConvertPlayerCount(string countText)
    {
        int result;
        if (!int.TryParse(countText, out result))
            return 0;
        else
            return result;
    }

    public void ShowRoomCreatedSuccess()
    {
        _passwordText.text = string.Empty;
        _awaitingCreateResponse = false;
        _canvasGroup.SetActive(false, true);
    }
    
    public void ShowRoomCreatedFailed()
    {
        _awaitingCreateResponse = false;
        _canvasGroup.SetActive(true, true);
    }

    public void ShowRoomJoinedSuccess()
    {
        _canvasGroup.SetActive(false, true);
    }

    public void ShowRoomJoinedFailed()
    {
        _canvasGroup.SetActive(true, true);
    }

    public void ShowRoomLeftSuccess()
    {
        _canvasGroup.SetActive(true, true);
    }

    public void ShowRoomLeftFailed()
    {
        _canvasGroup.SetActive(false, true);
    }
}
