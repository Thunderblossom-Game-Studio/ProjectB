using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine;

public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SyncObject]
    public readonly SyncList<Player> players = new SyncList<Player>();

    [SyncVar]
    public bool canStart;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!IsServer) return;

        canStart = players.All(player => player.isReady);

        Debug.Log("Can start: " + canStart);
    }

}
