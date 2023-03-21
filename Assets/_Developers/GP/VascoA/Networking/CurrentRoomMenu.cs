using FishNet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentRoomMenu : MonoBehaviour
{
    #region Types.
    /// <summary>
    /// Ways to process a room status response.
    /// </summary>
    private enum RoomProcessingTypes
    {
        Unset,
        Create,
        Join,
        Leave,
        Start
    }

    #endregion

    #region Public.
    /// <summary>
    /// Current member entries.
    /// </summary>
    [HideInInspector]
    public List<MemberEntry> MemberEntries = new List<MemberEntry>();
    #endregion

    #region Serialized.

    [SerializeField]
    private Button _startButton;
  
    [SerializeField]
    private Transform _membersContent;
    
    [SerializeField]
    private MemberEntry _memberEntryPrefab;
    #endregion

    #region Private.
 
    private CanvasGroup _canvasGroup;
    
    private bool _awaitingStartResponse = false;
 
    private bool _awaitingkickResponse = false;
    #endregion

   
    public void Initialize()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Reset();
    }

   
    public void Reset()
    {
        _awaitingkickResponse = false;
        _awaitingStartResponse = false;

        //Destroy children of content. This is just to get rid of any placeholders.
        foreach (Transform c in _membersContent)
            Destroy(c.gameObject);
        MemberEntries.Clear();
        ProcessRoomStatus(RoomProcessingTypes.Unset, false, null, string.Empty);
    }

    
    public void ShowRoomCreatedSuccess(RoomDetails roomDetails)
    {
        ProcessRoomStatus(RoomProcessingTypes.Create, true, roomDetails, string.Empty);
    }
    
    public void ShowRoomCreatedFailed(string failedReason)
    {
        ProcessRoomStatus(RoomProcessingTypes.Create, false, null, failedReason);
    }
 
    public void ShowRoomJoinedSuccess(RoomDetails roomDetails)
    {
        ProcessRoomStatus(RoomProcessingTypes.Join, true, roomDetails, string.Empty);
    }
    
    public void ShowRoomJoinedFailed(string failedReason)
    {
        ProcessRoomStatus(RoomProcessingTypes.Join, false, null, failedReason);
    }

    public void ShowRoomLeftSuccess()
    {
        ProcessRoomStatus(RoomProcessingTypes.Leave, true, null, string.Empty);
    }

    public void ShowRoomLeftFailed()
    {
        ProcessRoomStatus(RoomProcessingTypes.Leave, false, null, string.Empty);
    }

    public void ShowStartGame(bool success, RoomDetails roomDetails, string failedReason)
    {
        _awaitingStartResponse = false;
        ProcessRoomStatus(RoomProcessingTypes.Start, success, roomDetails, failedReason);
    }

    private void ProcessRoomStatus(RoomProcessingTypes processType, bool success, RoomDetails roomDetails, string failedReason)
    {
        bool hideCondition = (processType == RoomProcessingTypes.Unset) ||
            (processType == RoomProcessingTypes.Leave && success) ||
            (processType == RoomProcessingTypes.Start && success) ||
            (processType == RoomProcessingTypes.Join && !success) ||
            (processType == RoomProcessingTypes.Create && !success);
        bool show = !hideCondition;

        //Set active based on success.
        _canvasGroup.SetActive(show, true);
        //If hiding also destroy entries.
        if (!show)
            DestroyMemberEntries();

    
        UpdateStartButton();

        /* Don't update room actions canvas when a room starts.
         * This is the global canvas which allows players to
         * leave the room. Players should still be allowed to leave
         * after room starts. */
        bool updateRoomActions = (processType != RoomProcessingTypes.Start);
        if (updateRoomActions)
        {
            string roomName = (roomDetails == null) ? string.Empty : roomDetails.RoomName;
            //GlobalManager.CanvasesManager.RoomActionCanvas.ShowCurrentRoom((success && show), roomName);
        }

        //If failed to create room.
        if (failedReason != string.Empty)
            Debug.LogError(failedReason); //TODO VASCO
        //GlobalManager.CanvasesManager.MessagesCanvas.InfoMessages.ShowTimedMessage(failedReason, Color.red);
    }

    private void DestroyMemberEntries()
    {
        for (int i = 0; i < MemberEntries.Count; i++)
            Destroy(MemberEntries[i].gameObject);

        MemberEntries.Clear();
    }

    private void CreateMemberEntries(RoomDetails roomDetails)
    {
        DestroyMemberEntries();
        UpdateStartButton();

        bool host = LobbyManager.IsRoomHost(roomDetails, InstanceFinder.ClientManager.Connection.FirstObject);
        //Add current members to content.
        for (int i = 0; i < roomDetails.MemberIds.Count; i++)
        {
            MemberEntry entry = Instantiate(_memberEntryPrefab, _membersContent);
            entry.Initialize(this, roomDetails.MemberIds[i], roomDetails.StartedMembers.Contains(roomDetails.MemberIds[i]));
            /* Set kick active if member isnt self, match hasnt already started,
             * and if host. */
            entry.SetKickActive(
                roomDetails.MemberIds[i] != InstanceFinder.ClientManager.Connection.FirstObject &&
                host &&
                !roomDetails.IsStarted
                );

            MemberEntries.Add(entry);
        }
    }

    private void UpdateStartButton()
    {
        string startFailedString = string.Empty;
        _startButton.interactable = LobbyManager.CanUseStartButton(LobbyManager.CurrentRoom, InstanceFinder.ClientManager.Connection.FirstObject);
    }

    public void UpdateRoom(RoomDetails[] roomDetails)
    {
        RoomDetails currentRoom = LobbyManager.CurrentRoom;
        //Not in a room, nothing to update.
        if (currentRoom == null)
            return;

        for (int i = 0; i < roomDetails.Length; i++)
        {
            //Not current room.
            if (roomDetails[i].RoomName != currentRoom.RoomName)
                continue;

            /* It's easier to just re-add entries so 
             * that's what we'll do. */
            CreateMemberEntries(roomDetails[i]);
        }

    }
    
    public void OnClick_StartGame()
    {
        //Still waiting for a server response.
        if (_awaitingStartResponse)
            return;

        string failedReason = string.Empty;
        bool result = LobbyManager.CanStartRoom(LobbyManager.CurrentRoom, InstanceFinder.ClientManager.Connection.FirstObject, ref failedReason, false);
        if (!result)
        {
            //GlobalManager.CanvasesManager.MessagesCanvas.InfoMessages.ShowTimedMessage(failedReason, MessagesCanvas.BRIGHT_ORANGE);
        }
        else
        {
            _awaitingStartResponse = true;
            _startButton.interactable = false;
            LobbyManager.StartGame();
        }
    }
    
    public void KickMember(MemberEntry entry)
    {
        if (_awaitingkickResponse)
            return;
        if (entry.MemberId == null)
            return;

        _awaitingkickResponse = true;
        //Try to kick member Id on entry.
        LobbyManager.KickMember(entry.MemberId);
    }

    public void ProcessKickMemberSuccess()
    {
        _awaitingkickResponse = false;
    }
    
    public void ProcessKickMemberFailed(string failedReason)
    {
        _awaitingkickResponse = false;
        if (failedReason != string.Empty)
            Debug.LogError(failedReason); //TODO VASCO
            //GlobalManager.CanvasesManager.MessagesCanvas.InfoMessages.ShowTimedMessage(failedReason, Color.red);
    }

}

