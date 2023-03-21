using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemberEntry : MonoBehaviour
{
    public NetworkObject MemberId { get; private set; }

    public ClientInfo ClientInfo { get; private set; }


    [SerializeField] private Image _startedIcon;
    
    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private GameObject _kickButton;


    protected CurrentRoomMenu CurrentRoomMenu;

    public virtual void Initialize(CurrentRoomMenu crm, NetworkObject id, bool started)
    {
        _startedIcon.gameObject.SetActive(started);
        CurrentRoomMenu = crm;
        MemberId = id;
        ClientInfo = id.GetComponent<ClientInfo>();

        _name.text = ClientInfo.GetUsername();
    }

    public void SetKickActive(bool active)
    {
        _kickButton.SetActive(active);
    }

    public void OnClick_Kick()
    {
        CurrentRoomMenu.KickMember(this);
    }
}
