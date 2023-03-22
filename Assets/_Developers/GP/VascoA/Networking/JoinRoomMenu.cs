using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomMenu : MonoBehaviour
{
    [SerializeField] private Transform _roomsContent;

    [SerializeField] private RoomEntry _roomEntryPrefab;

    [SerializeField] private CanvasGroup _passwordCanvas;

    [SerializeField] private TMPro.TMP_InputField _passwordInputField;


    private CanvasGroup _canvasGroup;

    private List<RoomEntry> _roomEntries = new List<RoomEntry>();

    private bool _awaitingPasswordResponse = false;

    private string _cachedRoomName = string.Empty;

    
    public void Initialize()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Reset();
    }

    public void Reset()
    {
        ShowRoomJoinedFailed();
        _passwordCanvas.SetActive(false, true);

        for (int i = 0; i < _roomEntries.Count; i++)
            Destroy(_roomEntries[i].gameObject); //TODO VASCO Foreach

        _roomEntries.Clear();
    }

    public void ShowRoomCreatedSuccess()
    {
        _canvasGroup.SetActive(false, true);
    }

    public void ShowRoomJoinedSuccess()
    {
        _awaitingPasswordResponse = false;
        _passwordCanvas.SetActive(false, true);
        _canvasGroup.SetActive(false, true);
    }

    public void ShowRoomJoinedFailed()
    {
        _awaitingPasswordResponse = false;
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

    public void UpdateRooms(RoomDetails[] roomDetails)
    {
        for (int i = 0; i < roomDetails.Length; i++)
        {
            int index = ReturnRoomEntriesIndex(roomDetails[i].RoomName);
            
            if(index == -1)
            {
                if (roomDetails[i].MemberIds.Count == 0 || 
                    roomDetails[i].MemberIds.Count >= roomDetails[i].MaxPlayers ||
                    roomDetails[i].IsStarted)
                    continue;

                RoomEntry entry = Instantiate(_roomEntryPrefab, _roomsContent);
                entry.Initialize(this, roomDetails[i]);
                _roomEntries.Add(entry);
            }
            else
            {
                _roomEntries[index].Initialize(this, roomDetails[i]);

                if(_roomEntries[index].RoomDetails.MemberIds.Count == 0)
                {
                    Destroy(_roomEntries[index].gameObject);
                    _roomEntries.RemoveAt(index);
                }
                else
                {
                    if (_roomEntries[index].RoomDetails.MemberIds.Count >= 
                        _roomEntries[index].RoomDetails.MaxPlayers ||
                            _roomEntries[index].RoomDetails.IsStarted)
                    {
                        Destroy(_roomEntries[index].gameObject);
                        _roomEntries.RemoveAt(index);
                    }
                }
            }
        }
    }

    private int ReturnRoomEntriesIndex(string roomName)
    {
        //Use a for loop instead of linq to avoid allocations.
        for (int i = 0; i < _roomEntries.Count; i++)
        {
            if (_roomEntries[i].RoomDetails.RoomName == roomName)
                return i;
        }

        //Fall through, not found.
        return -1;
    }


    public void JoinRoom(RoomEntry roomEntry)
    {
        _cachedRoomName = roomEntry.RoomDetails.RoomName;
        if(roomEntry.RoomDetails.IsPasswordProtected)
        {
            _passwordInputField.text = string.Empty;
            _passwordCanvas.SetActive(true, true);
        }
        else
        {
            LobbyManager.JoinRoom(_cachedRoomName, string.Empty);
        }
    }

    public void OnClick_CancelPassword()
    {
        _passwordCanvas.SetActive(false, true);
    }

    public void OnClick_JoinPassword()
    {
        if (_awaitingPasswordResponse)
            return;

        string password = _passwordInputField.text;
        LobbyManager.SanitizeTextMeshProString(ref password);
        if (string.IsNullOrEmpty(password))
        {
            //TODO VASCO
            Debug.LogError("A password is required to join this room!");
            //GlobalManager.CanvasesManager.MessagesCanvas.InfoMessages.ShowTimedMessage("A password is required to join this room.", MessagesCanvas.BRIGHT_ORANGE);
        }
        //Try to join with supplied password.
        else
        {
            LobbyManager.JoinRoom(_cachedRoomName, password);
        }
    }
}
