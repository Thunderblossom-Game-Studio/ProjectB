using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : BaseMenu<GameMenu>
{
    [SerializeField] private CanvasGroup buttonHolder;

    public override IEnumerator OpenMenuRoutine(Action OnComplected = null)
    {
        yield return FeelUtility.FadeCanvasGroup(buttonHolder, new FeelFloatProperties(1, .5f, animationCurveType: AnimationCurveType.EaseInOut));

        InputManager.Instance.SwithControlMode(InputManager.ControlMode.Gameplay);
        
        InputManager.Instance.OnPauseAction += Instance_OnPauseAction;

        yield return base.OpenMenuRoutine(OnComplected);
    }

    public override IEnumerator CloseMenuRoutine(Action OnComplected = null)
    {
        yield return FeelUtility.FadeCanvasGroup(buttonHolder, new FeelFloatProperties(0, .2f, animationCurveType: AnimationCurveType.EaseInOut));

        InputManager.Instance.OnPauseAction -= Instance_OnPauseAction;

        yield return base.CloseMenuRoutine(OnComplected);
    }

    public void MenuButtonPressed()
    {
        CloseMenu(() => (LoadingMenu.Instance as LoadingMenu).LoadScene(0));
    }

    public void OptionButtonPressed()
    {
        InputManager.Instance.SwithControlMode(InputManager.ControlMode.UI);
        PauseMenu.Open();
    }

    private void Instance_OnPauseAction(object sender, EventArgs e)
    {
        if (PauseMenu.Instance && PauseMenu.IsOpened) return;
        PauseMenu.Open();
    }
}
