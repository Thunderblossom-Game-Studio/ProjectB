using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    private static LobbyManager Instance;

    protected LobbyCanvasManager LobbyCanvasManager;

    public Dictionary<NetworkConnection, string> LoggedInUsernames = new Dictionary<NetworkConnection, string>();

    public event Action<NetworkObject> OnClientLoggedIn;

    public static event Action<NetworkObject> OnMemberJoined;
    public static event Action<NetworkObject> OnMemberStarted;
    public static event Action<NetworkObject> OnMemberLeft;

    public List<RoomDetails> CreatedRooms = new List<RoomDetails>();

    public Dictionary<NetworkConnection, RoomDetails> ConnectionRooms = new Dictionary<NetworkConnection, RoomDetails>();

    public event Action<RoomDetails, NetworkObject> OnClientLeftRoom;

    public event Action<RoomDetails, NetworkObject> OnClientJoinedRoom;

    

    public RoomDetails currentRoom { get; private set; } = null;
    public static RoomDetails CurrentRoom
    {
        get { return Instance.currentRoom; }
        private set { Instance.currentRoom = value; }
    }

    private const int MIN_PLAYERS = 2;
    private const int MAX_PLAYERS = 6;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        Instance = this;

        LobbyCanvasManager = GameObject.FindObjectOfType<LobbyCanvasManager>();
        if (LobbyCanvasManager == null)
        {
            Debug.LogError("LobbyCanvasManager not found.");
            return;
        }
        LobbyCanvasManager.Initialize();


        InstanceFinder.SceneManager.OnClientLoadedStartScenes += OnClientLoadedStartScenes;
    }

    #region Subscribed Events

    private void OnClientLoadedStartScenes(NetworkConnection conn, bool asServer)
    {


    }

    #endregion



    #region SignIn

    [Client]
    public static void SignIn(string username)
    {
        Instance.InternalSignIn(username);
    }

    private void InternalSignIn(string username)
    {
        CmdSignIn(username);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CmdSignIn(string username, NetworkConnection sender = null)
    {
        ClientInfo ci;
        if (!FindClientInstance(sender, out ci))
            return;

        string failedReason = string.Empty;
        bool success = OnSignIn(ref username, ref failedReason);



    }

    private bool OnSignIn(ref string username, ref string failedReason)
    {
        throw new NotImplementedException();
    }

    public static bool SanitizeUsername(ref string value, ref string failedReason) { return Instance.InternalSanitizeUsername(ref value, ref failedReason); }


    protected virtual bool InternalSanitizeUsername(ref string value, ref string failedReason)
    {
        value = value.Trim();
        SanitizeTextMeshProString(ref value);

        if (value.Length < 3)
        {
            failedReason = "Username must be at least 3 characters long.";
            return false;
        }

        if (value.Length > 15)
        {
            failedReason = "Username can't be longer than 15 characters long.";
            return false;
        }

        bool letters = value.All(c => Char.IsLetter(c));
        if (!letters)
        {
            failedReason = "Username can only contain letters.";
            return false;
        }

        return true;
    }

    #endregion

    #region Join Room

    [Client]
    public static void JoinRoom(string roomName, string password)
    {
        Instance.JoinRoomInternal(roomName, password);
    }
    
    private void JoinRoomInternal(string roomName, string password)
    {
        CmdJoinRoom(roomName, password);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void CmdJoinRoom(string roomName, string password, NetworkConnection sender = null)
    {
        ClientInfo ci;
        if (!FindClientInstance(sender, out ci))
            return;

        string failedReason = string.Empty;
        RoomDetails roomDetails = null;

        bool success = OnJoinRoom(roomName, password, ci.NetworkObject, ref failedReason, ref roomDetails);

        if (success)
        {
            roomDetails.AddMember(ci.NetworkObject);
            ConnectionRooms[ci.Owner] = roomDetails;
            TargetJoinRoomSuccess(ci.Owner, roomDetails);
            OnClientJoinedRoom?.Invoke(roomDetails, ci.NetworkObject);

            RpcUpdateRooms(new RoomDetails[] { roomDetails });

            foreach (NetworkObject item in roomDetails.MemberIds)
                TargetMemberJoined(item.Owner, ci.NetworkObject);
        }
        else
        {
            //Send failed reason to client.
            TargetJoinRoomFailed(ci.Owner, failedReason);
        }
    }

    protected virtual bool OnJoinRoom(string roomName, string password, NetworkObject joiner, ref string failedReason, ref RoomDetails roomDetails)
    {
        //Already in a room. 
        if (ReturnRoomDetails(joiner) != null)
        {
            failedReason = "You are already in a room.";
            return false;
        }

        roomDetails = ReturnRoomDetails(roomName);
        //Room doesn't exist.
        if (roomDetails == null)
        {
            failedReason = "Room does not exist.";
            return false;
        }
        //Room exist.
        else
        {
            //Wrong password.
            if (roomDetails.IsPasswordProtected && password != roomDetails.Password)
            {
                failedReason = "Incorrect room password.";
                return false;
            }
            //Full.
            if (roomDetails.MemberIds.Count >= roomDetails.MaxPlayers)
            {
                failedReason = "Room is full.";
                return false;
            }
            //If started and locked on start.
            if (roomDetails.IsStarted)
            {
                failedReason = "Room has already started.";
                return false;
            }
            //Kicked from room.
            if (roomDetails.IsMemberKicked(joiner))
            {
                failedReason = "You are kicked from that room.";
                return false;
            }
        }

        //All checks passed.
        return true;
    }

    [TargetRpc]
    private void TargetMemberJoined(NetworkConnection conn, NetworkObject member)
    {
        //Not in a room, shouldn't have got this. Likely left as someone joined.
        if (CurrentRoom == null)
            return;

        MemberJoined(member);
    }

    private void MemberJoined(NetworkObject member)
    {
        CurrentRoom.AddMember(member);
        OnMemberJoined?.Invoke(member);
    }

    [TargetRpc]
    private void TargetJoinRoomSuccess(NetworkConnection conn, RoomDetails roomDetails)
    {
        CurrentRoom = roomDetails;
        //TODO VASCO
        //LobbyCanvasManager.CreateJoinCurrentRoomCanvas.ShowRoomJoinedSuccess(roomDetails);
    }

    [TargetRpc]
    private void TargetJoinRoomFailed(NetworkConnection conn, string failedReason)
    {
        CurrentRoom = null;
        //TODO VASCO
        //LobbyCanvasManager.CreateJoinCurrentRoomCanvas.ShowRoomJoinedFailed(failedReason);
    }
    #endregion

    #region Create Room

    [Client]
    public static void CreateRoom(string roomName, string password, int playerCount) { Instance.InternalCreateRoom(roomName, password, playerCount); }

    private void InternalCreateRoom(string roomName, string password, int playerCount)
    {
        CmdCreateRoom(roomName, password, playerCount);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CmdCreateRoom(string roomName, string password, int playerCount, NetworkConnection sender = null)
    {
        ClientInfo ci;
        if (!FindClientInstance(sender, out ci))
            return;

        bool success = false;
        string failedReason = string.Empty;

        // TODO VASCO rest of this

    }

    public static bool SanitizePlayerCount(int count, ref string failedReason) { return Instance.OnSanitizePlayerCount(count, ref failedReason); }

    protected virtual bool OnSanitizePlayerCount(int count, ref string failedReason)
    {
        if (count < OnReturnMinimumPlayers() || count > OnReturnMaximumPlayers())
        {
            failedReason = "Invalid player count.";
            return false;
        }

        return true;
    }

    public static int ReturnMinimumPlayers() { return Instance.OnReturnMinimumPlayers(); }

    protected virtual int OnReturnMinimumPlayers() { return MIN_PLAYERS; }

    public static int ReturnMaximumPlayers() { return Instance.OnReturnMaximumPlayers(); }

    protected virtual int OnReturnMaximumPlayers() { return MAX_PLAYERS; }

    public static bool SanitizeRoomName(ref string value, ref string failedReason) { return Instance.OnSanitizeRoomName(ref value, ref failedReason); }

    private bool OnSanitizeRoomName(ref string value, ref string failedReason)
    {
        value = value.Trim();
        SanitizeTextMeshProString(ref value);

        if (value.Length > 25)
        {
            failedReason = "Room name must be at most 25 characters long.";
            return false;
        }
        if (value.Length < 3)
        {
            failedReason = "Room name must be at least 3 characters long.";
            return false;
        }
        bool letters = value.All(c => Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c));
        if (!letters)
        {
            failedReason = "Room name may only contain letters and numbers.";
            return false;
        }

        return true;
    }

    #endregion

    #region Leave Room
    [TargetRpc]
    private void TargetMemberLeft(NetworkConnection conn, NetworkObject member)
    {
        //Not in a room, shouldn't have got this. Likely left as someone joined.
        if (CurrentRoom == null)
            return;

        CurrentRoom.RemoveMember(member);
        OnMemberLeft?.Invoke(member);
    }
    #endregion

    #region Manage Rooms

    private RoomDetails ReturnRoomDetails(string roomName)
    {
        foreach (RoomDetails room in CreatedRooms)
        {
            if (room.RoomName.Equals(roomName, System.StringComparison.CurrentCultureIgnoreCase))
                return room;
        }

        return null;
    }

    private RoomDetails ReturnRoomDetails(NetworkObject clientId)
    {
        foreach (RoomDetails room in CreatedRooms)
        {
            for (int i = 0; i < room.MemberIds.Count; i++)
            {
                if (room.MemberIds[i] == clientId)
                    return room;
            }
        }

        return null;
    }

    [Server]
    private RoomDetails RemoveFromRoom(NetworkObject clientId, bool clientDisconnected)
    {
        RoomDetails roomDetails = ReturnRoomDetails(clientId);
        if (roomDetails != null)
        {
            foreach (NetworkObject member in roomDetails.MemberIds)
            {
                if (clientDisconnected && member == clientId)
                    continue;

                TargetMemberLeft(member.Owner, member);
            }

            roomDetails.RemoveMember(clientId);
            ConnectionRooms.Remove(clientId.Owner);

            OnClientLeftRoom?.Invoke(roomDetails, clientId);

            //If client didn't disconnect tell client to unload scenes.
            if (!clientDisconnected)
            {
                SceneLookupData[] lookups = SceneLookupData.CreateData(roomDetails.Scenes.ToArray());
                SceneUnloadData sud = new SceneUnloadData(lookups);

                if (lookups.Length > 0)
                    InstanceFinder.SceneManager.UnloadConnectionScenes(clientId.Owner, sud);
            }

            //Delete room if it is empty
            if (roomDetails.MemberIds.Count == 0)
                CreatedRooms.Remove(roomDetails);

            if (!roomDetails.IsStarted)
                RpcUpdateRooms(new RoomDetails[] { roomDetails });

        }

        return roomDetails;
    }

    [ObserversRpc]
    public void RpcUpdateRooms(RoomDetails[] roomDetails)
    {
        if (CurrentRoom != null)
        {
            for (int i = 0; i < roomDetails.Length; i++)
            {
                if (roomDetails[i].RoomName == CurrentRoom.RoomName)
                {
                    CurrentRoom = roomDetails[i];
                    break;
                }
            }
        }

        /* This runs on clients even if they are in a match. It may
             * be optimal to only send this to clients which are not in a match,
             * or ignore if in a match then request room updates upon
             * leaving the match. */
        
        //LobbyCanvases.JoinCreateRoomCanvas.CurrentRoomMenu.UpdateRoom(roomDetails);
        //LobbyCanvases.JoinCreateRoomCanvas.JoinRoomMenu.UpdateRooms(roomDetails);

    }

    private void SendRooms(NetworkConnection conn)
    {
        List<RoomDetails> rooms = new List<RoomDetails>();
        foreach (RoomDetails room in CreatedRooms)
            rooms.Add(room);

        if (rooms.Count > 0)
            TargetInitialRooms(conn, rooms.ToArray());
    }

    [TargetRpc]
    public void TargetInitialRooms(NetworkConnection conn, RoomDetails[] roomDetails)
    {

    }

    #endregion

    #region Utilities
    /// <summary>
    /// Textmesh pro seems to add on an unknown char at the end.
    /// If last char is an invalid ascii then remove it.
    /// </summary>
    /// <param name="value"></param>
    public static void SanitizeTextMeshProString(ref string value)
    {
        if (value.Length == 0)
            return;

        if ((int)value[value.Length - 1] > 255)
            value = value.Substring(0, value.Length - 1);
    }

    /// <summary>
    /// Finds and and outputs the ClientInstance for a connection.
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="ci"></param>
    /// <returns>True if ClientInfo is found on connection.</returns>
    private bool FindClientInstance(NetworkConnection conn, out ClientInfo ci)
    {
        ci = null;
        if (conn == null)
        {
            Debug.LogError("Connection is null.");
            return false;
        }

        ci = ClientInfo.ReturnClientInstance(conn);
        if (ci == null)
        {
            Debug.LogError("ClientInfo not found on connection.");
            return false;
        }

        return true;
    }
    #endregion

















}
