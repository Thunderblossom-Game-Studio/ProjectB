using UnityEngine;

public class LobbyCanvas : MonoBehaviour
{
    public LobbyCanvasManager LobbyCanvasManager { get; private set; }

    
    [SerializeField] private GameObject playerCardPrefab;
    [SerializeField] private GameObject playerCardContainer;
    [SerializeField] private TMPro.TMP_Text infoText;



    public void Initialize(LobbyCanvasManager lobbyCanvasManager)
    {
        LobbyCanvasManager = lobbyCanvasManager;
    }

    

    public void AddPlayerCard(string username)
    {
        var go = Instantiate(playerCardPrefab, playerCardContainer.transform);
        go.GetComponentInChildren<TMPro.TMP_Text>().text = username;
    }

    public void RemovePlayerCard(string username)
    {
        var go = playerCardContainer.transform.Find(username);
        if (go != null)
            Destroy(go.gameObject);
    }

    
}
