using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pelumi.Juicer;


public class GameOverHUD : BaseMenu<GameOverHUD>
{ 
    [SerializeField] private CanvasGroup buttonHolder;

    [Header("Buttons")]

    [SerializeField] private AdvanceButton menuButton;
    [SerializeField] private AdvanceButton quitButton;

    private void Start()
    {

        menuButton.onClick.AddListener(MenuButton);
        quitButton.onClick.AddListener(QuitButton);

    }

    public override IEnumerator OpenMenuRoutine(Action onCompleted = null)
    {
        yield return Juicer.DoFloat(() => buttonHolder.interactable = true, buttonHolder.alpha, (v) => buttonHolder.alpha = v, new JuicerFloatProperties(1, .5f, animationCurveType: AnimationCurveType.EaseInOut));
        InputManager.Instance.SwithControlMode(InputManager.ControlMode.UI);

        yield return base.OpenMenuRoutine(onCompleted);
    }

    public override IEnumerator CloseMenuRoutine(Action onCompleted = null)
    {
        yield return Juicer.DoFloat(() => buttonHolder.interactable = false, buttonHolder.alpha, (v) => buttonHolder.alpha = v, new JuicerFloatProperties(0, .5f, animationCurveType: AnimationCurveType.EaseInOut));
        yield return base.CloseMenuRoutine(onCompleted);
    }




    public void MenuButton()
    {
        Close(() => LoadingMenu.GetInstance().LoadScene((int)SceneType.MainMenu));
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