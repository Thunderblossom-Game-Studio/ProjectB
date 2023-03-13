using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    private const int MIN_NAME_LENGTH = 3;
    [SerializeField] private TMPro.TMP_InputField usernameInputField;
    [SerializeField] private LobbyManager lobbyManager;
  
    public void OnClick_Accept()
    {
        if (usernameInputField.text.Length > MIN_NAME_LENGTH)
        {
            lobbyManager.SignIn(usernameInputField.text);
            Debug.Log("Username Accepted");
            
        }
    }
}
