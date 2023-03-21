using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Transporting;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class ReadyLobbyManager : LobbyManager
{
    public static event Action<NetworkObject, bool> OnMemberSetReady;

    #region Private.
   
    private Dictionary<RoomDetails, HashSet<NetworkObject>> _serverReadyPlayers = new Dictionary<RoomDetails, HashSet<NetworkObject>>();
    
    private HashSet<NetworkObject> _clientReadyPlayers = new HashSet<NetworkObject>();
    
    private static ReadyLobbyManager Instance;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        base.OnClientLeftRoom += MyLobbyNetwork_OnClientLeftRoom;
        base.OnClientJoinedRoom += ReadyLobbyNetwork_OnClientJoinedRoom;
        InstanceFinder.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;

        LobbyManager.OnMemberLeft += LobbyNetwork_OnMemberLeft;
        InstanceFinder.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
    }

    private void OnDestroy()
    {
        if (InstanceFinder.ClientManager == null)
            return;
        InstanceFinder.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
        LobbyManager.OnMemberLeft -= LobbyNetwork_OnMemberLeft;
    }

    private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
    {
        if (obj.ConnectionState == LocalConnectionState.Started)
            return;

        //Clear local ready players.
        _clientReadyPlayers.Clear();

        /* Unload all scenes except lobby for client.
         * Since they are disconnected this doesn't ened to be done through
         * FSM. Also, FSM scene changes MUST only be done via server,
         * this is being run on client. */
        for (int i = 0; i < UnitySceneManager.sceneCount; i++)
        {
            Scene s = UnitySceneManager.GetSceneAt(i);
            if (s != gameObject.scene)
                UnitySceneManager.UnloadSceneAsync(s);
        }

        base.LobbyCanvasManager.SetLobbyCameraActive(true); //TODO VASCO
    }

    private void SceneManager_OnLoadEnd(SceneLoadEndEventArgs obj)
    {
        if (!obj.QueueData.AsServer)
            return;

        /* When the server loads a scene try to find the SceneRoomDetails script in it
         * and pass in the RoomDetails for the scene loaded. This isn't required by any
         * means but it shows how you can have a reference to the RoomDetails which
         * the scene is for on the server. */
        object[] p = obj.QueueData.SceneLoadData.Params.ServerParams;
        if (p != null && p.Length > 1)
        {
            RoomDetails rd = (RoomDetails)p[1];
            //Try to find script in scene.
            foreach (Scene s in obj.LoadedScenes)
            {
                GameObject[] gos = s.GetRootGameObjects();
                for (int i = 0; i < gos.Length; i++)
                {
                    //If found.
                    //if (gos[i].TryGetComponent<GameplayManager>(out GameplayManager gpm))
                    //{
                    //    gpm.FirstInitialize(rd, this);
                    //    break;
                    //}
                    //TODO VASCO

                }
            }
        }
    }

    private void ReadyLobbyNetwork_OnClientJoinedRoom(RoomDetails roomDetails, NetworkObject joiner)
    {
        HashSet<NetworkObject> readyPlayers;
        //If any players have readied up send it to joining player.
        if (_serverReadyPlayers.TryGetValue(roomDetails, out readyPlayers))
        {
            foreach (NetworkObject item in readyPlayers)
                TargetSetReady(joiner.Owner, item, true);
        }
    }

    [Client]
    public static void SetReady(bool ready)
    {
        Instance.SetReadyInternal(ready);
    }
    private void SetReadyInternal(bool ready)
    {
        CmdSetReady(ready);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CmdSetReady(bool ready, NetworkConnection sender = null)
    {
        SetReady(ready, sender.FirstObject, true);
    }

    private void SetReady(bool ready, NetworkObject changingPlayer, bool asServer)
    {
        //Running on server.
        if (asServer)
        {
            RoomDetails roomDetails;
            //Not in a room.
            if (!base.ConnectionRooms.TryGetValue(changingPlayer.Owner, out roomDetails))
            {
                Debug.LogWarning($"Cannot ready client as they are not in a room.");
            }
            //In a room, find room in ready list and add player.
            else
            {
                HashSet<NetworkObject> readyPlayers;
                //If not found make new hashset.
                if (!_serverReadyPlayers.TryGetValue(roomDetails, out readyPlayers))
                {
                    readyPlayers = new HashSet<NetworkObject>();
                    _serverReadyPlayers[roomDetails] = readyPlayers;
                }

                if (ready)
                    readyPlayers.Add(changingPlayer);
                else
                    readyPlayers.Remove(changingPlayer);

                foreach (NetworkObject item in roomDetails.MemberIds)
                    TargetSetReady(item.Owner, changingPlayer, ready);
            }
        }
        //Running on client.
        else
        {
            if (ready)
                _clientReadyPlayers.Add(changingPlayer);
            else
                _clientReadyPlayers.Remove(changingPlayer);

            OnMemberSetReady?.Invoke(changingPlayer, ready);
        }
    }

    [TargetRpc]
    private void TargetSetReady(NetworkConnection conn, NetworkObject identity, bool ready)
    {
        SetReady(ready, identity, false);
    }

    private void MyLobbyNetwork_OnClientLeftRoom(RoomDetails roomDetails, NetworkObject leaver)
    {
        //If no more members in room then try to remove from readyplayers.
        if (roomDetails.MemberIds.Count == 0)
        {
            _serverReadyPlayers.Remove(roomDetails);
        }
        //If still has members then remove leaving member.
        else
        {
            if (_serverReadyPlayers.TryGetValue(roomDetails, out HashSet<NetworkObject> idents))
                idents.Remove(leaver);
        }
    }

    private void LobbyNetwork_OnMemberLeft(NetworkObject obj)
    {
        /* If self that left then clear ready players.
         * Otherwise remove leaving player. */
        if (obj == InstanceFinder.ClientManager.Connection.FirstObject)
            _clientReadyPlayers.Clear();
        else
            _clientReadyPlayers.Remove(obj);
    }

    protected override bool OnCanStartRoom(RoomDetails roomDetails, NetworkObject startingPlayer, ref string failedReason, bool asServer)
    {
        //Something in base script prevented starting.
        if (!base.OnCanStartRoom(roomDetails, startingPlayer, ref failedReason, asServer))
            return false;

        //Not enough members to start room.
        if (roomDetails.MemberIds.Count < 1)
        {
            failedReason = "Not enough members to start.";
            return false;
        }

        /* If match has already started and this far then lock on start is false,
         * meaning players can join and leave as they wish. At this point there is
         * no reason to require players ready since the game started. */
        if (roomDetails.IsStarted)
            return true;

        //Try to get current ready for this room. If doesn't exist yet then make.
        HashSet<NetworkObject> readyPlayers;
        //If running as server then use server hashset.
        if (asServer)
        {
            if (!_serverReadyPlayers.TryGetValue(roomDetails, out readyPlayers))
            {
                readyPlayers = new HashSet<NetworkObject>();
                _serverReadyPlayers[roomDetails] = readyPlayers;
            }
        }
        //Otherwise use client hashset.
        else
        {
            readyPlayers = _clientReadyPlayers;
        }

        /* In the base class only host can start the room; though, you
        * very well could override that. For this example we will not,
        * and it would be redundant to make host ready while also clicking
        * start. Instead I will check if startingPlayer is host, and if so,
        * automatically add them to ready players. */
        if (roomDetails.MemberIds[0] == startingPlayer)
            SetReady(true, startingPlayer, asServer);

        //Ready players count is same as member count for room.
        if (readyPlayers.Count == roomDetails.MemberIds.Count)
        {
            return true;
        }
        else
        {
            failedReason = "Not all players are ready.";
            return false;
        }
    }
}
