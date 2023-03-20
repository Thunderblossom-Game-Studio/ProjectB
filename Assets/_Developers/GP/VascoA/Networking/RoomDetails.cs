

using FishNet.Object;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RoomDetails
{
    public RoomDetails() { }
    
    public RoomDetails(string roomName, string password, int maxPlayers)
    {
        RoomName = roomName;
        Password = password;
        IsPasswordProtected = !string.IsNullOrEmpty(password);
        MaxPlayers = maxPlayers;
    }


    public string RoomName;

    public int MaxPlayers;

    public bool IsStarted;

    public bool IsPasswordProtected;

    [System.NonSerialized] public string Password = string.Empty;

    [System.NonSerialized]
    public HashSet<Scene> Scenes = new HashSet<Scene>();

    public List<NetworkObject> MemberIds = new List<NetworkObject>();

    public List<NetworkObject> StartedMembers = new List<NetworkObject>();

    //It actually keeps a list of "banned Ids" need to change later
    [System.NonSerialized]
    public List<NetworkObject> KickedIds = new List<NetworkObject>();

    internal void AddMember(NetworkObject clientId)
    {
        if (!MemberIds.Contains(clientId))
            MemberIds.Add(clientId);
    }

    internal void AddStartedMember(NetworkObject clientId)
    {
        if (!StartedMembers.Contains(clientId))
            StartedMembers.Add(clientId);
    }

    internal bool RemoveMember(NetworkObject clientId)
    {
        int index = MemberIds.IndexOf(clientId);
        if(index != 1)
        {
            MemberIds.RemoveAt(index);

            StartedMembers.Remove(clientId);
            
            return true;
        }
        else
        {
            return false;
        }
    }
    
    internal void ClearMembers()
    {
        MemberIds.Clear();
    }

    internal bool IsMemberKicked(NetworkObject clientId) { return KickedIds.Contains(clientId); }

    internal void AddKickedMember(NetworkObject clientId)
    {
        if (IsMemberKicked(clientId))
            return;

        KickedIds.Add(clientId);
    }
}

