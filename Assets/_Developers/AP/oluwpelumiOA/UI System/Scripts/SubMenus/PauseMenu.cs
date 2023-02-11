using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : BaseMenu<PauseMenu>
{
    [SerializeField] private CanvasGroup buttonHolder;

    [Header("Buttons")]
    [SerializeField] private AdvanceButton restartButton;
    [SerializeField] private AdvanceButton resumeButton;
    [SerializeField] private AdvanceButton settingsButton;
    [SerializeField] private AdvanceButton menuButton;
    [SerializeField] private AdvanceButton quitButton;

    private void Start()
    {
        restartButton.onClick.AddListener(RestartButton);
        resumeButton.onClick.AddListener(ResumeButton);
        settingsButton.onClick.AddListener(SettingsButton);
        menuButton.onClick.AddListener(MenuButton);
        quitButton.onClick.AddListener(QuitButton);
    }

    public override IEnumerator OpenMenuRoutine(Action OnComplected = null)
    {
        yield return FeelUtility.FadeFloat(() => buttonHolder.interactable = true, buttonHolder.alpha, (v) => buttonHolder.alpha = v, new FeelFloatProperties(1, .5f, animationCurveType: AnimationCurveType.EaseInOut));
        InputManager.Instance.SwithControlMode(InputManager.ControlMode.UI);

        yield return base.OpenMenuRoutine(OnComplected);
    }

    public override IEnumerator CloseMenuRoutine(Action OnComplected = null)
    {
        yield return FeelUtility.FadeFloat(() => buttonHolder.interactable = false, buttonHolder.alpha, (v) => buttonHolder.alpha = v, new FeelFloatProperties(0, .5f, animationCurveType: AnimationCurveType.EaseInOut));
        yield return base.CloseMenuRoutine(OnComplected);
    }

    public override void OnBackPressed()
    {
        
    }

    public void RestartButton()
    {
        Close(() => (LoadingMenu.Instance as LoadingMenu).LoadScene(1));
    }

    public void ResumeButton()
    {
        Close(()=>InputManager.Instance.SwithControlMode(InputManager.ControlMode.Gameplay));
    }

    public void SettingsButton()
    {
        SettingsMenu.Open();
    }

    public void MenuButton()
    {
        Close(() => (LoadingMenu.Instance as LoadingMenu).LoadScene(0));
        GameMenu.Close();
    }
    
    public void QuitButton()
    {
        Application.Quit();
    }

    public static void ToggleMenu()
    {
        if (Instance && IsOpened) Close(); else Open();
    }
}
