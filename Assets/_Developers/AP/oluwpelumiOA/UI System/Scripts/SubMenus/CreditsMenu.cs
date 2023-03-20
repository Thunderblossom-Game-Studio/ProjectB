using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pelumi.Juicer;
using UnityEngine.SceneManagement;

public class CreditsMenu : BaseMenu<CreditsMenu>
{
    
    
    [Header("Buttons")]
    [SerializeField] private AdvanceButton backButton;

    private void Start()
    {
        backButton.onClick.AddListener(CloseButton);
    }

    protected override void Instance_OnTabLeftAction(object sender, EventArgs e)
    {

    }

    protected override void Instance_OnTabRightAction(object sender, EventArgs e)
    {

    }

    protected override void Instance_OnBackAction(object sender, EventArgs e)
    {
        CloseButton();
    }

    public void CloseButton()
    {
        Close(() => lastMenu.OpenMenu());
    }
}
