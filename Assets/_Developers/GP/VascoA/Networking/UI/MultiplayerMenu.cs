using FishNet;
using UnityEngine;
using UnityEngine.UI;
public class MultiplayerMenu : MonoBehaviour
{
    [SerializeField] private Button _hostButton;
    
    [SerializeField] private Button _joinButton;

    private void Start()
    {
        _hostButton.onClick.AddListener(() =>
        {
            InstanceFinder.ServerManager.StartConnection();
            
            InstanceFinder.ClientManager.StartConnection();
        });

        _joinButton.onClick.AddListener(() => InstanceFinder.ClientManager.StartConnection());
 
    }

}
