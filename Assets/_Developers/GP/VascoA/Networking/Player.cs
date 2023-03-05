using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public sealed class Player : NetworkBehaviour
{
    [SyncVar]
    public string username;

    [SyncVar]
    public bool isReady;

    public override void OnStartServer()
    {
        base.OnStartServer();

        GameManager.Instance.players.Add(this);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        GameManager.Instance.players.Remove(this);
    }

    private void Update()
    {
        if (!IsOwner) return;

        if(Input.GetKeyDown(KeyCode.R))
        {
            ServerSetIsReady(!isReady);
        }
    }

    [ServerRpc]
    public void ServerSetIsReady(bool value)
    {
        isReady = value;
    }

}
