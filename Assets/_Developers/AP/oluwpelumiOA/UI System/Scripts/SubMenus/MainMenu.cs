using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pelumi.Juicer;
using UnityEngine.SceneManagement;

public class MainMenu : BaseMenu<MainMenu>
{
    [SerializeField] private Transform view1;
    [SerializeField] private CanvasGroup buttonHolder;

    [Header("Buttons")]
    [SerializeField] private AdvanceButton startButton;
    [SerializeField] private AdvanceButton carSelectButton;
    [SerializeField] private AdvanceButton settingsButton;
    [SerializeField] private AdvanceButton quitButton;
    [SerializeField] private AdvanceButton creditButton;
    [SerializeField] private AdvanceButton templevelButton;

    [Header("Open Transition Settings")]
    [SerializeField] private JuicerVector3Properties view1OpenTransition;

    [Header("Close Transition Settings")]
    [SerializeField] private JuicerVector3Properties view1CloseTransition;

    private void Start()
    {
        startButton.onClick.AddListener(StartButton);
        carSelectButton.onClick.AddListener(CarSelectButton);
        settingsButton.onClick.AddListener(OptionButton);
        quitButton.onClick.AddListener(QuitButton);
        creditButton.onClick.AddListener(CreditButton);
        templevelButton.onClick.AddListener(TempLevelButton);
    }

    public override IEnumerator OpenMenuRoutine(Action onCompleted = null)
    {
        yield return Juicer.DoVector3(null, Vector3.zero, (pos) => view1.transform.localScale = pos,  view1OpenTransition, null);
        yield return Juicer.DoFloat(null, 0, (pos) => buttonHolder.alpha = pos, new JuicerFloatProperties(1, .2f, animationCurveType: AnimationCurveType.EaseInOut), null);

        yield return base.OpenMenuRoutine(onCompleted);
    }

    public override IEnumerator CloseMenuRoutine(Action onCompleted = null)
    {

        yield return Juicer.DoFloat(null, buttonHolder.alpha, (pos) => buttonHolder.alpha = pos, new JuicerFloatProperties(0, .2f, animationCurveType: AnimationCurveType.EaseInOut), null);
        yield return Juicer.DoVector3(null, view1.transform.localScale, (pos) => view1.transform.localScale = pos, view1CloseTransition, null);

        yield return base.CloseMenuRoutine(onCompleted);
    }

    protected override void ResetUI()
    {
        view1.localScale = Vector3.zero;
        buttonHolder.alpha = 0;
    }

    public void StartButton()
    {
        Close(() => SceneSelectMenu.Open());
    }

    public void CarSelectButton()
    {
        Close(() => CarSelectMenu.Open());
    }

    public void OptionButton()
    {
        Close(() => SettingsMenu.Open());
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void CreditButton()
    {
        //Close(() => SceneManager.LoadScene("Credits", LoadSceneMode.Single));

        Close(() => (LoadingMenu.Instance as LoadingMenu)?.LoadScene(6));
    }

    public void TempLevelButton()
    {
        Close(() => TempLevelSelect.Open());
    }
}
