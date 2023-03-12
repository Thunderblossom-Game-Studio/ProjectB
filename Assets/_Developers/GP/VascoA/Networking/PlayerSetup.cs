using FishNet;
using FishNet.Object;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private TMPro.TMP_InputField usernameInputField;

   
    public void OnClick_Accept()
    {
        if (usernameInputField.text.Length > 0)
        {
            
        }
    }
}
