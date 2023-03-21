using FishNet;
using FishNet.Object;
using UnityEngine;
using UnityEngine.UI;

public class ReadyMemberEntry : MemberEntry
{
    [SerializeField] Image _readyImage = null;
    
    [SerializeField] Image _readyButtonImage = null;

    [SerializeField] Color _readyColor = Color.green;

    [SerializeField] Color _notReadyColor = Color.red;


    private bool _ready = false;

    private bool _awaitingReadyResponse = false;
    
    
    public override void Initialize(CurrentRoomMenu crm, NetworkObject id, bool started)
    {
        base.Initialize(crm, id, started);

        NetworkObject localNob = InstanceFinder.ClientManager.Connection.FirstObject;

        bool local = (id == localNob);
        if (local)
        {
            if(LobbyManager.CurrentRoom.IsStarted || LobbyManager.IsRoomHost(LobbyManager.CurrentRoom, localNob))
            {
                _readyButtonImage.gameObject.SetActive(false);
                _readyImage.gameObject.SetActive(true);
                SetLocalReady(true);
            }
            else
            {
                _readyButtonImage.gameObject.SetActive(true);
                _readyImage.gameObject.SetActive(false);

            }
        }
        else
        {
            _readyButtonImage.gameObject.SetActive(false);
            _readyImage.gameObject.SetActive(false);
        }     
    }

    private void Awake()
    {
        ReadyLobbyManager.OnMemberSetReady += ReadyLobbyNetwork_OnMemberSetReady;
    }

    private void OnDestroy()
    {
        ReadyLobbyManager.OnMemberSetReady -= ReadyLobbyNetwork_OnMemberSetReady;
    }

    private void ReadyLobbyNetwork_OnMemberSetReady(NetworkObject arg1, bool ready)
    {
        //Not for this member.
        if (arg1 != base.MemberId)
            return;

        //For local client.
        if (base.MemberId == InstanceFinder.ClientManager.Connection.FirstObject)
        {
            _awaitingReadyResponse = false;
            Color c = (ready) ? _readyColor : _notReadyColor;
            _readyButtonImage.color = c;
            _ready = ready;
        }
        //For another player.
        else
        {
            _readyImage.gameObject.SetActive(ready);
        }
    }

    public void OnClick_Ready()
    {
        SetLocalReady(!_ready);
    }

    private void SetLocalReady(bool ready)
    {
        //Don't do anything if not for local client. Only local client should be able to click this anyway.
        if (base.MemberId != InstanceFinder.ClientManager.Connection.FirstObject)
            return;
        if (_awaitingReadyResponse)
            return;

        _awaitingReadyResponse = true;
        ReadyLobbyManager.SetReady(!_ready);
    }
}
