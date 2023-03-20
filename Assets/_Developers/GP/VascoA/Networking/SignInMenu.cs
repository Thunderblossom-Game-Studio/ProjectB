using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignInMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameText;
    [SerializeField] private Button _signInButton;


    private CanvasGroup _canvasGroup;
    private PlayerSignInCanvas _playerSignInCanvas;

    public void Initialize(PlayerSignInCanvas playerSignInCanvas)
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _playerSignInCanvas = playerSignInCanvas;
        Reset();
    }

    public void Reset()
    {
        SignInFailed(string.Empty);
    }

    public void OnClick_SignIn()
    {
        string username = _usernameText.text.Trim();
        string failedReason = string.Empty;

        if(!LobbyManager.SanitizeUsername(ref username, ref failedReason))
        {
            // TODO VASCO MessageCanvas
            Debug.Log(failedReason);
        }
        else
        {
            SetSignInLocked(true);
            LobbyManager.SignIn(username);
        }
    }

    private void SetSignInLocked(bool locked)
    {
        _signInButton.interactable = !locked;
        _usernameText.enabled = !locked;
    }

    public void SignInSuccess()
    {
        _canvasGroup.SetActive(false, true);
        SetSignInLocked(false);
    }

    public void SignInFailed(string failedReason)
    {
        _canvasGroup.SetActive(true, true);
        SetSignInLocked(false);

        if (failedReason != string.Empty)
        {
            // TODO VASCO MessageCanvas
            Debug.Log(failedReason);
        }
    }

    
}
