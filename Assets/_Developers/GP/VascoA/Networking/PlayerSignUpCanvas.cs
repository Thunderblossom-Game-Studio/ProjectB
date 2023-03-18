using UnityEngine;

public class PlayerSignUpCanvas : MonoBehaviour
{
    public LobbyCanvasManager LobbyCanvasManager { get; private set; }


    private const int MIN_NAME_LENGTH = 3;
    [SerializeField] private TMPro.TMP_InputField usernameInputField;
    [SerializeField] private LobbyManager lobbyManager;
  


    public void Initialize(LobbyCanvasManager lobbyCanvasManager)
    {
        LobbyCanvasManager = lobbyCanvasManager;
    }



    public void OnClick_Accept()
    {
        if (usernameInputField.text.Length > MIN_NAME_LENGTH)
        {
            string username = usernameInputField.text.Trim();

            LobbyManager.SignIn(username);

            this.gameObject.SetActive(false);

        }
    }
}
