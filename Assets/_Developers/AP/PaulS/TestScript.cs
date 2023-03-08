using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private ConfirmationWindow myConfirmationWindow;
    public string PopUpMessage;
    
    void Start()
    {
        OpenConfirmationWindow(PopUpMessage);
    }

    private void OpenConfirmationWindow(string message)
    {
        myConfirmationWindow.gameObject.SetActive(true);
        myConfirmationWindow.yesButton.onClick.AddListener(YesClicked);
        myConfirmationWindow.noButton.onClick.AddListener(NoClicked);
        myConfirmationWindow.messageText.text = message;
    }

    private void YesClicked()
    {
        myConfirmationWindow.gameObject.SetActive(false);
        myConfirmationWindow.yesButton.onClick.RemoveListener(YesClicked);
        //Open next menu
        Debug.Log("Yes Clicked");
    }

    private void NoClicked()
    {
        myConfirmationWindow.gameObject.SetActive(false);
        myConfirmationWindow.noButton.onClick.RemoveListener(NoClicked);
        Debug.Log("No Clicked");
    }
}
