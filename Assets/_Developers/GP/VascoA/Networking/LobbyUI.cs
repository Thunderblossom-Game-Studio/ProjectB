using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private GameObject playerCardPrefab;
    [SerializeField] private GameObject playerCardContainer;


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

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
