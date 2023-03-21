using FishNet;
using FishNet.Connection;
using FishNet.Managing.Logging;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Transporting;
using FishNet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    private static LobbyManager Instance;

    private enum ParamsTypes
    {
        ServerLoad,
        MemberLeft
    }

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

    public event Action<RoomDetails, NetworkObject> OnClientStarted;

    public event Action<RoomDetails, NetworkObject> OnClientCreatedRoom;

    public event Action<RoomDetails, SceneLoadEndEventArgs> OnServerLoadedScenes;

    [Tooltip("GameSceneConfigurations to get loading scenes from.")]
    [SerializeField]
    private GameSceneConfigurations _gameSceneConfigurations;

    public RoomDetails currentRoom { get; private set; } = null;
    public static RoomDetails CurrentRoom
    {
        get { return Instance.currentRoom; }
        private set { Instance.currentRoom = value; }
    }

    private const int MIN_PLAYERS = 2;
    private const int MAX_PLAYERS = 6;

    #region Initialization.
    protected virtual void Awake()
    {
        Initialize();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        //Load lobby as a global scene so all players join it upon connecting.
        //SceneLoadData sld = new SceneLoadData(gameObject.scene);
        //base.NetworkManager.SceneManager.LoadGlobalScenes(sld);
        //Unsubscribe first incase Mirror is dumb and never calls OnStopServer. Mirror likes to do stupid things like this.
        ChangeSubscriptions(false);
        ChangeSubscriptions(true);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        ChangeSubscriptions(false);
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


        InstanceFinder.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
        InstanceFinder.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
        InstanceFinder.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
        InstanceFinder.ServerManager.OnRemoteConnectionState += ServerManager_OnRemoteConnectionState;
    }

    private void SceneManager_OnClientLoadedStartScenes(NetworkConnection arg1, bool asServer)
    {
        if (asServer)
            SendRooms(arg1);
    }

    /// <summary>
    /// Called after the local server connection state changes.
    /// </summary>
    private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
    {
        ServerReset();
        //Also reset client incase acting as client host.
        ClientReset();
    }

    /// <summary>
    /// Called when a client state changes with the server.
    /// </summary>
    private void ServerManager_OnRemoteConnectionState(NetworkConnection arg1, RemoteConnectionStateArgs arg2)
    {
        if (arg2.ConnectionState == RemoteConnectionState.Stopped)
            ClientDisconnected(arg1);
    }

    /// <summary>
    /// Called after the local client connection state changes.
    /// </summary>
    private void ClientManager_OnClientConnectionState(FishNet.Transporting.ClientConnectionStateArgs obj)
    {
        if (!ApplicationState.IsQuitting() && obj.ConnectionState != FishNet.Transporting.LocalConnectionState.Started)
            ClientReset();
    }


    /// <summary>
    /// Changes subscriptions needed to operate.
    /// </summary>
    /// <param name="subscribe"></param>
    private void ChangeSubscriptions(bool subscribe)
    {
        if (base.NetworkManager == null)
            return;

        if (subscribe)
        {
            base.NetworkManager.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;
            base.NetworkManager.SceneManager.OnClientPresenceChangeEnd += SceneManager_OnClientPresenceChangeEnd;
        }
        else
        {
            base.NetworkManager.SceneManager.OnLoadEnd -= SceneManager_OnLoadEnd;
            base.NetworkManager.SceneManager.OnClientPresenceChangeEnd -= SceneManager_OnClientPresenceChangeEnd;
        }
    }

    #endregion

    #region SceneManager callbacks.
    /// <summary>
    /// Called when a clients presence changes for a scene.
    /// </summary>
    private void SceneManager_OnClientPresenceChangeEnd(ClientPresenceChangeEventArgs obj)
    {
        if (obj.Added)
            HandleClientLoadedScene(obj);
    }

    /// <summary>
    /// Called after one or more scenes have loaded.
    /// </summary>
    private void SceneManager_OnLoadEnd(SceneLoadEndEventArgs obj)
    {
        if (obj.QueueData.AsServer)
            HandleServerLoadedScenes(obj);

    }
    #endregion

    #region NetworkManager callbacks.
    /// <summary>
    /// Received after a client has it's player instantiated.
    /// </summary>
    /// <param name="obj"></param>
    private void LobbyAndWorldNetworkManager_RelayOnServerAddPlayer(NetworkConnection conn)
    {
        SendRooms(conn);
    }

    private void ClientDisconnected(NetworkConnection obj)
    {
        ClientLeftServer(obj);
        LoggedInUsernames.Remove(obj);
        ConnectionRooms.Remove(obj);
    }

    /// <summary>
    /// Received when the server stops.
    /// </summary>
    private void LobbyAndWorldNetworkManager_RelayOnStopServer()
    {
        ServerReset();
        //Also reset client incase acting as client host.
        ClientReset();
    }

    /// <summary>
    /// Resets client as though just connecting. Can be called from server as ClientHost. Intended to reset client settings when they disconnect from the server. This is not required if quitting the game.
    /// </summary>
    //[Client]
    /* [Client] attribute may be bugged. Even though this method definitely calls from client it's being blocked as Mirror believes the server is making the call.
     * Perhaps it's because LobbyNetwork is server owned?. */
    private void ClientReset()
    {
        LobbyCanvasManager.Reset();
        CurrentRoom = null;
        //Can be null if stopping server.
        ClientInfo ci = ClientInfo.ReturnClientInstance(null);
        if (ci != null)
            ci.SetUsername(string.Empty);
    }

    /// <summary>
    /// Resets server as though just starting.
    /// </summary>
    private void ServerReset()
    {
        CreatedRooms.Clear();
        LoggedInUsernames.Clear();
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
        if (success)
        {
            //Add to usernames on server.
            LoggedInUsernames[ci.Owner] = username;
            ci.SetUsername(username);
            OnClientLoggedIn?.Invoke(ci.NetworkObject);
            TargetSignInSuccess(ci.Owner, username);
        }
        else
        {
            TargetSignInFailed(ci.Owner, failedReason);
        }


    }

    private bool OnSignIn(ref string username, ref string failedReason)
    {
        //Didn't pass sanitization.
        if (!SanitizeUsername(ref username, ref failedReason))
            return false;

        //Check if in logged in users already.
        foreach (KeyValuePair<NetworkConnection, string> item in LoggedInUsernames)
        {
            //Username is already taken.
            if (item.Value.ToLower() == username.ToLower())
            {
                failedReason = "Username is already taken.";
                return false;
            }
        }

        //All checks passed.
        return true;
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

    [TargetRpc]
    private void TargetSignInSuccess(NetworkConnection conn, string username)
    {
        LobbyCanvasManager.PlayerSignInCanvas.SignInSuccess(username);
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.SignInSuccess();
    }

    [TargetRpc]
    private void TargetSignInFailed(NetworkConnection conn, string failedReason)
    {
        if (failedReason == string.Empty)
            failedReason = "Sign in failed.";
        LobbyCanvasManager.PlayerSignInCanvas.SignInFailed(failedReason);
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.SignInFailed();
    }

    #endregion

    #region Start Room

    [Client(Logging = LoggingType.Off)]
    public static bool CanUseStartButton(RoomDetails roomDetails, NetworkObject clientId)
    {
        return Instance.OnCanUseStartButton(roomDetails, clientId);
    }

    protected virtual bool OnCanUseStartButton(RoomDetails roomDetails, NetworkObject clientId)
    {
        //RoomDetails is null; shouldn't happen but may under extremely rare race conditions.
        if (roomDetails == null)
            return false;
        //Joined room after it started and is lock on start. Shouldn't be possible.
        if (roomDetails.IsStarted)
            return false;
        /* Not host, and room hasn't started yet.
         * Only host can initialize first start. */
        if (!IsRoomHost(roomDetails, clientId) && !roomDetails.IsStarted)
            return false;

        return true;
    }

    public static bool CanStartRoom(RoomDetails roomDetails, NetworkObject clientId, ref string failedReason, bool asServer)
    {
        return Instance.OnCanStartRoom(roomDetails, clientId, ref failedReason, asServer);
    }
    
    protected virtual bool OnCanStartRoom(RoomDetails roomDetails, NetworkObject clientId, ref string failedReason, bool asServer)
    {
        //RoomDetails is null; shouldn't happen but may under extremely rare race conditions.
        if (roomDetails == null)
        {
            failedReason = "Room information is missing.";
            return false;
        }
        //Joined room after it started and is lock on start. Shouldn't be possible.
        if (roomDetails.IsStarted)
        {
            failedReason = "Room has already started. Try another room.";
            return false;
        }
        /* Not host, and room hasn't started yet.
         * Only host can initialize first start. */
        if (!IsRoomHost(roomDetails, clientId) && !roomDetails.IsStarted)
        {
            failedReason = "You are not the host of your current room.";
            return false;
        }
        //Not enough players.
        if (roomDetails.MemberIds.Count < 1)
        {
            failedReason = "There must be at least two players to start a game.";
            return false;
        }
        //No configured scenes.
        string[] scenes = _gameSceneConfigurations.GetGameScenes();
        if (scenes == null || scenes.Length == 0)
        {
            failedReason = "No scenes are specified as the game scene.";
            return false;
        }

        //All checks have passed.
        return true;
    }

    public static bool IsRoomHost(RoomDetails roomDetails, NetworkObject clientId)
    {
        if (roomDetails == null || roomDetails.MemberIds == null || roomDetails.MemberIds.Count == 0)
            return false;

        return (roomDetails.MemberIds[0] == clientId);
    }

    [Client]
    public static void StartGame()
    {
        Instance.StartGameInternal();
    }
    
    private void StartGameInternal()
    {
        CmdStartGame();
    }

    [ServerRpc(RequireOwnership = false)]
    private void CmdStartGame(NetworkConnection sender = null)
    {
        ClientInfo ci;
        if (!FindClientInstance(sender, out ci))
            return;

        RoomDetails roomDetails = ReturnRoomDetails(ci.NetworkObject);
        string failedReason = string.Empty;
        bool success = OnCanStartRoom(roomDetails, ci.NetworkObject, ref failedReason, true);

        //If still successful.
        if (success)
        {
            /* If game has not yet started then set it up. */
            if (!roomDetails.IsStarted)
            {
                //Set started immediately.
                roomDetails.IsStarted = true;
                SceneLoadData sld = new SceneLoadData(_gameSceneConfigurations.GetGameScenes());
                LoadOptions loadOptions = new LoadOptions
                {
                    LocalPhysics = _gameSceneConfigurations.PhysicsMode,
                    AllowStacking = true,
                };
                LoadParams loadParams = new LoadParams
                {
                    ServerParams = new object[]
                     {
                             ParamsTypes.ServerLoad,
                             roomDetails,
                             sld
                     }
                };
                sld.Options = loadOptions;
                sld.Params = loadParams;

                /* Only load scene for the server. This is to ensure that the server
                 * can load the scene fine, and once it does it will load for clients. */
                InstanceFinder.SceneManager.LoadConnectionScenes(sld);
            }
            /* If game has started then we must be sure to not re-initialize everything on server.
             * Only new client needs to be caught up. */
            else
            {
                SceneLookupData[] lookups = SceneLookupData.CreateData(roomDetails.Scenes.ToArray());
                SceneLoadData sld = new SceneLoadData(lookups);
                //Load for joining client.
                InstanceFinder.SceneManager.LoadConnectionScenes(ci.Owner, sld);
            }
        }
        //Failed.
        else
        {
            //Inform person trying to start that response is failed.
            TargetStartGameFailed(ci.Owner, roomDetails, failedReason);
        }
    }

    private void HandleClientLoadedScene(ClientPresenceChangeEventArgs args)
    {
        /* If client is loading into lobby scene then there's no reason
         * to check if they are in a room or not. */
        if (args.Scene == gameObject.scene)
            return;

        NetworkConnection joinerConn = args.Connection;
        
        if (ConnectionRooms.TryGetValue(args.Connection, out RoomDetails roomDetails))
        {
            /* Only need to initialize 'started' if not already started.
             * This method will run even when multiple scenes are being loaded.
             * So if two scenes were loaded for the game then this would run twice. */
            NetworkObject firstObject = joinerConn.FirstObject;
            if (!roomDetails.StartedMembers.Contains(firstObject))
            {
                roomDetails.AddStartedMember(firstObject);
                OnClientStarted?.Invoke(roomDetails, firstObject);
                TargetLeaveLobby(joinerConn, roomDetails);
                //Send that args.Connection joined to all other members.
                foreach (NetworkObject item in roomDetails.MemberIds)
                {
                    if (item.Owner != joinerConn)
                        TargetMemberStarted(item.Owner, firstObject);
                }
                /* Send all members to args.Connection,
                 * that is if there are other members. */
                if (roomDetails.StartedMembers.Count > 1)
                    TargetMembersStarted(joinerConn, roomDetails.StartedMembers);

            }
        }
        //RoomDetails not found, tell client to unload the scene.
        else
        {
            SceneUnloadData sud = new SceneUnloadData(args.Scene.name);
            InstanceFinder.SceneManager.UnloadConnectionScenes(joinerConn, sud);
            /* Also tell the client they successfully left the room 
             * so they clean up everything on their end. */
            TargetLeaveRoomSuccess(joinerConn);
        }
    }

    [TargetRpc]
    private void TargetMemberStarted(NetworkConnection conn, NetworkObject member)
    {
        //Not in a room, shouldn't have got this. Likely left as someone joined.
        if (CurrentRoom == null)
            return;

        CurrentRoom.AddStartedMember(member);
        OnMemberStarted?.Invoke(member);
    }

    [TargetRpc]
    private void TargetMembersStarted(NetworkConnection conn, List<NetworkObject> members)
    {
        //Not in a room, shouldn't have got this. Likely left as someone joined.
        if (CurrentRoom == null)
            return;

        for (int i = 0; i < members.Count; i++)
            CurrentRoom.AddStartedMember(members[i]);
    }

    private void HandleServerLoadedScenes(SceneLoadEndEventArgs args)
    {
        object[] parameters = args.QueueData.SceneLoadData.Params.ServerParams;
        //No parameters. This can occur when loading for client after server has loaded the scene.
        if (parameters == null || parameters.Length == 0)
            return;

        ParamsTypes pt = (ParamsTypes)parameters[0];
        //Should never happen but check just in case.
        if (pt == ParamsTypes.ServerLoad)
            ServerLoadedScene(args);
    }

    private void ServerLoadedScene(SceneLoadEndEventArgs args)
    {
        LoadParams lp = args.QueueData.SceneLoadData.Params;
        if (lp == null || lp.ServerParams.Length < 2)
            return;

        object[] parameters = lp.ServerParams;
        RoomDetails roomDetails = (RoomDetails)parameters[1];

        //Connection for who is room host.
        NetworkConnection roomHost = null;
        //Find room host.
        if (roomDetails.MemberIds.Count > 0 && roomDetails.MemberIds[0] != null)
            roomHost = roomDetails.MemberIds[0].Owner;

        /* Scenes werent loaded and none were skipped.
         * Shouldn't happen, but add response just incase. */
        if (args.LoadedScenes.Length == 0 && (args.SkippedSceneNames == null || args.SkippedSceneNames.Length == 0))
        {
            //Tell starter than creation failed. first index in members will always be host.
            if (roomHost != null)
                TargetStartGameFailed(roomHost, roomDetails, "Server failed to load game scene.");

            return;
        }

        /* If here then scenes were loaded. */
        HashSet<Scene> scenes = new HashSet<Scene>();
        foreach (Scene s in args.LoadedScenes)
            scenes.Add(s);
        /* Scenes must be stored in the roomdetails so the server knows
         * which to unload when the room is empty. This data only exist on the
         * server. */
        roomDetails.Scenes = scenes;
        OnServerLoadedScenes?.Invoke(roomDetails, args);

        //Load clients into scenes.
        NetworkConnection[] conns = new NetworkConnection[roomDetails.MemberIds.Count];
        for (int i = 0; i < roomDetails.MemberIds.Count; i++)
            conns[i] = roomDetails.MemberIds[i].Owner;
        //Build sceneloaddata.
        SceneLoadData sld = new SceneLoadData(args.LoadedScenes);
        LoadOptions lo = new LoadOptions()
        {
            AllowStacking = true,
        };
        sld.Options = lo;
        //Load connections in.
        InstanceFinder.SceneManager.LoadConnectionScenes(conns, sld);

        //Update rooms to clients so that their lobby is up to date.
        RpcUpdateRooms(new RoomDetails[] { roomDetails });
    }

    [TargetRpc]
    private void TargetStartGameFailed(NetworkConnection conn, RoomDetails roomDetails, string failedReason)
    {
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.ShowStartGame(false, roomDetails, failedReason);
    }

    [TargetRpc]
    private void TargetLeaveLobby(NetworkConnection conn, RoomDetails roomDetails)
    {
        LobbyCanvasManager.SetLobbyCameraActive(false);
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.ShowStartGame(true, roomDetails, string.Empty);
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
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.ShowRoomJoinedSuccess(roomDetails);
    }

    [TargetRpc]
    private void TargetJoinRoomFailed(NetworkConnection conn, string failedReason)
    {
        CurrentRoom = null;
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.ShowRoomJoinedFailed(failedReason);
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
        if (ReturnRoomDetails(ci.NetworkObject) != null)
            failedReason = "You are already in a room.";
        else
            success = OnCreateRoom(ref roomName, password, playerCount, ref failedReason);

        //If nothing failed.
        if (success)
        {
            /* Make a new room details.
             * Add creator to members and
             * assign room name. */
            RoomDetails roomDetails = new RoomDetails(roomName, password, playerCount);
            roomDetails.AddMember(ci.NetworkObject);
            CreatedRooms.Add(roomDetails);
            ConnectionRooms[ci.Owner] = roomDetails;

            OnClientCreatedRoom?.Invoke(roomDetails, ci.NetworkObject);
            TargetCreateRoomSuccess(ci.Owner, roomDetails);
            RpcUpdateRooms(new RoomDetails[] { roomDetails });
        }
        //Room creation failed.
        else
        {
            TargetCreateRoomFailed(ci.Owner, failedReason);
        }
    }

    protected virtual bool OnCreateRoom(ref string roomName, string password, int playerCount, ref string failedReason)
    {
        if (!SanitizeRoomName(ref roomName, ref failedReason))
            return false;
        if (!SanitizePlayerCount(playerCount, ref failedReason))
            return false;

        if (InstanceFinder.IsServer)
        {
            RoomDetails roomDetails = ReturnRoomDetails(roomName);
            if (roomDetails != null)
            {
                failedReason = "Room name already exist.";
                return false;
            }
        }

        //All checks passed.
        return true;
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

    [TargetRpc]
    private void TargetCreateRoomSuccess(NetworkConnection conn, RoomDetails roomDetails)
    {
        CurrentRoom = roomDetails;
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.ShowRoomCreatedSuccess(roomDetails);
        //Also send member joined to self.
        MemberJoined(InstanceFinder.ClientManager.Connection.FirstObject);
    }

    [TargetRpc]
    private void TargetCreateRoomFailed(NetworkConnection conn, string failedReason)
    {
        CurrentRoom = null;
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.ShowRoomCreatedFailed(failedReason);
    }

    #endregion

    #region Leave Room

    [Server]
    private void ClientLeftServer(NetworkConnection conn)
    {
        if (FindClientInstance(conn, out ClientInfo ci))
            RemoveFromRoom(ci.NetworkObject, true);
    }

    [Client]
    public static void LeaveRoom()
    {
        Instance.LeaveRoomInternal();
    }

    private void LeaveRoomInternal()
    {
        if (CurrentRoom != null)
            CmdLeaveRoom();
    }

    [ServerRpc(RequireOwnership = false)]
    private void CmdLeaveRoom(NetworkConnection sender = null)
    {
        ClientInfo ci;
        if (!FindClientInstance(sender, out ci))
            return;

        TryLeaveRoom(ci.NetworkObject);
    }

    [Server]
    public void TryLeaveRoom(NetworkObject clientId)
    {
        RoomDetails roomDetails = RemoveFromRoom(clientId, false);
        bool success = (roomDetails != null);

        if (success)
            TargetLeaveRoomSuccess(clientId.Owner);
        else
            TargetLeaveRoomFailed(clientId.Owner);
    }

    [TargetRpc]
    private void TargetLeaveRoomSuccess(NetworkConnection conn)
    {
        LobbyCanvasManager.SetLobbyCameraActive(true);
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.ShowRoomLeftSuccess();
    }

    [TargetRpc]
    private void TargetLeaveRoomFailed(NetworkConnection conn)
    {
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.ShowRoomLeftFailed();
    }

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

        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.CurrentRoomMenu.UpdateRoom(roomDetails);
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.JoinRoomMenu.UpdateRooms(roomDetails);
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
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.JoinRoomMenu.UpdateRooms(roomDetails);
    }

    #endregion

    #region Kick Player

    [Client]
    public static void KickMember(NetworkObject target)
    {
        Instance.InternalKickMember(target);
    }

    private void InternalKickMember(NetworkObject target)
    {
        string failedReason = string.Empty;
        if (OnCanKickMember(CurrentRoom, InstanceFinder.ClientManager.Connection.FirstObject, target, ref failedReason))
            CmdKickMember(target);
        else
            Debug.LogWarning(failedReason); //TODO VASCO
    }

    [ServerRpc(RequireOwnership = false)]
    private void CmdKickMember(NetworkObject target, NetworkConnection sender = null)
    {
        ClientInfo ci;
        if (!FindClientInstance(sender, out ci))
            return;

        NetworkObject kicker = ci.NetworkObject;
        RoomDetails targetRoom = ReturnRoomDetails(target);
        RoomDetails kickerRoom = ReturnRoomDetails(kicker);
        if (targetRoom != null && kickerRoom != null)
        {
            //If trying to kick someone in a different room simply debug locally and ignore client.
            if (kickerRoom != targetRoom)
            {
                Debug.LogWarning("Client is trying to kick members from a different room.");
                return;
            }
        }
        else
        {
            /* Kicker or target isn't in a room.
             * This might happen if leaving as a kick occurs. */
            if (kickerRoom != targetRoom)
            {
                Debug.LogWarning("Kicker or target is not in a room.");
                return;
            }
        }

        string failedReason = string.Empty;
        if (OnCanKickMember(kickerRoom, kicker, target, ref failedReason))
        {
            kickerRoom.AddKickedMember(target);
            TryLeaveRoom(target);
            TargetKickMemberSuccess(kicker.Owner);
        }
        else
        {
            TargetKickMemberFailed(kicker.Owner, failedReason);
        }
    }

    protected virtual bool OnCanKickMember(RoomDetails roomDetails, NetworkObject kicker, NetworkObject target, ref string failedReason)
    {
        if (!IsRoomHost(roomDetails, kicker))
            failedReason = "Only host may kick.";

        return (failedReason == string.Empty);
    }

    [TargetRpc]
    private void TargetKickMemberSuccess(NetworkConnection conn)
    {
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.CurrentRoomMenu.ProcessKickMemberSuccess();
    }

    private void TargetKickMemberFailed(NetworkConnection conn, string failedReason)
    {
        if (failedReason == string.Empty)
            failedReason = "Kick failed.";
        LobbyCanvasManager.CreateJoinCurrentRoomCanvas.CurrentRoomMenu.ProcessKickMemberFailed(failedReason);
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
