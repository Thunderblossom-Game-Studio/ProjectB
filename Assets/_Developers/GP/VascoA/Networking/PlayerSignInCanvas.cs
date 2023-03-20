using UnityEditor.TerrainTools;
using UnityEngine;

public class PlayerSignInCanvas : MonoBehaviour
{
    public LobbyCanvasManager LobbyCanvasManager { get; private set; }

    [SerializeField]  public SignInMenu _signInMenu;
    public SignInMenu SignInMenu { get { return _signInMenu; } }



    public void Initialize(LobbyCanvasManager lobbyCanvasManager)
    {
        LobbyCanvasManager = lobbyCanvasManager;
        SignInMenu.Initialize(this);
        Reset();
    }

    public void Reset()
    {
        SignInMenu.Reset();
        //TODO VASCO User Action Canvas
    }

    public void SignInSuccess(string username)
    {
        SignInMenu.SignInSuccess();
        //TODO VASCO User Action Canvas
    }

    public void SignInFailed(string failedReason)
    {
        SignInMenu.SignInFailed(failedReason);
        //TODO VASCO User Action Canvas
    }

    
}


