using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    public RoomDetails RoomDetails { get; private set; }


    [SerializeField] Image _passwordedImage = null;

    [SerializeField] TextMeshProUGUI _roomNameText = null;

    [SerializeField] TextMeshProUGUI _playerCountText = null;


    private JoinRoomMenu _joinRoomMenu;


    public void Initialize(JoinRoomMenu joinRoomMenu, RoomDetails roomDetails)
    {
        _joinRoomMenu = joinRoomMenu;
        RoomDetails = roomDetails;

        _roomNameText.text = roomDetails.RoomName;
        _playerCountText.text = roomDetails.MemberIds.Count + " / " + roomDetails.MaxPlayers;
        _passwordedImage.gameObject.SetActive(roomDetails.IsPasswordProtected);
    }

    public void OnClick_Button()
    {
        /* Make sure room isn't full. Shouldn't be
         * displayed if it is but check anyway. */
        if (RoomDetails.MemberIds.Count >= RoomDetails.MaxPlayers)
            return;

        _joinRoomMenu.JoinRoom(this);
    }

}
