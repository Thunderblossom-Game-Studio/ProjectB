using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pelumi.Juicer;
using UnityEngine.SceneManagement;

public class SceneSelectMenu : BaseMenu<SceneSelectMenu>
{
    [Header("Buttons")]
    [SerializeField] private AdvanceButton tutorialButton;
    [SerializeField] private AdvanceButton mpButton;
    [SerializeField] private AdvanceButton backButton;
    [SerializeField] private AdvanceButton TempLevel1;
    [SerializeField] private AdvanceButton TempLevel2;
    [SerializeField] private AdvanceButton TempLevel3;
    


    [SerializeField] private CanvasGroup buttonHolder;

    private void Start()
    {
        tutorialButton.onClick.AddListener(() => SwitchScene(SceneType.Tutorial));
        mpButton.onClick.AddListener(() => SwitchScene(SceneType.Multiplayer));
        backButton.onClick.AddListener(CloseButton);
        TempLevel1.onClick.AddListener(() => SwitchScene(SceneType.TempLevel1));
        TempLevel2.onClick.AddListener(() => SwitchScene(SceneType.TempLevel2));
        TempLevel3.onClick.AddListener(() => SwitchScene(SceneType.TempLevel3));
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
    Credits = 1,
    Tutorial = 2,
    Multiplayer = 3,
    TempLevel1 = 3,
    TempLevel2 = 4,
    TempLevel3 = 5,

}
