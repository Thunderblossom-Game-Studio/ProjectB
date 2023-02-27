using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pelumi.Juicer;
public class SceneSelectMenu : BaseMenu<SceneSelectMenu>
{
    [Header("Buttons")]
    [SerializeField] private AdvanceButton levelButton;
    [SerializeField] private AdvanceButton leve2Button;
    [SerializeField] private AdvanceButton mpButton;
    [SerializeField] private AdvanceButton backButton;
    

    [SerializeField] private CanvasGroup buttonHolder;

    private void Start()
    {
        levelButton.onClick.AddListener(() => SwitchScene(SceneType.Level1));
        leve2Button.onClick.AddListener(() => SwitchScene(SceneType.Level2));
        mpButton.onClick.AddListener(() => SwitchScene(SceneType.Multiplayer));
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

    public override IEnumerator OpenMenuRoutine(Action onCompleted = null)
    {
        yield return Juicer.DoFloat(null, buttonHolder.alpha, (v) => buttonHolder.alpha = v, new JuicerFloatProperties(1, .5f, animationCurveType: AnimationCurveType.EaseInOut));

        yield return base.OpenMenuRoutine(onCompleted);
    }

    public override IEnumerator CloseMenuRoutine(Action onCompleted = null)
    {
        yield return Juicer.DoFloat(null, buttonHolder.alpha, (v) => buttonHolder.alpha = v, new JuicerFloatProperties(0, .2f, animationCurveType: AnimationCurveType.EaseInOut));

        yield return base.CloseMenuRoutine(onCompleted);
    }

    protected override void ResetUI()
    {
        buttonHolder.alpha = 0;
    }

    public void CloseButton()
    {
        Close(() => lastMenu.OpenMenu());
    }

    public void SwitchScene(SceneType sceneType)
    {
        Close(() => LoadingMenu.GetInstance().LoadScene((int)sceneType));
    }
}

public enum SceneType
{
    MainMenu = 0,
    Level1 = 1,
    Level2 = 2,
    Multiplayer = 3
}
