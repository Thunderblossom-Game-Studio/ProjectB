using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Pelumi.Juicer;

public class NotifyWindow : BaseMenu<NotifyWindow>
{
    [SerializeField] private CanvasGroup content;
    [SerializeField] private TextMeshProUGUI headerDisplay;
    [SerializeField] private TextMeshProUGUI bodyDisplay;

    private Action onConfirmAction;

    public override IEnumerator OpenMenuRoutine(Action OnComplected = null)
    {
        yield return Juicer.DoFloat(null, 0, (pos) => content.alpha = pos, new JuicerFloatProperties(1, .2f, animationCurveType: AnimationCurveType.EaseInOut), () => ToggleLoadingContent(true));


        yield return base.OpenMenuRoutine(OnComplected);
    }

    public override IEnumerator CloseMenuRoutine(Action OnComplected = null)
    {
        yield return Juicer.DoFloat(null, content.alpha, (pos) => content.alpha = pos, new JuicerFloatProperties(0, .2f, animationCurveType: AnimationCurveType.EaseInOut), () => ToggleLoadingContent(false));

        yield return base.CloseMenuRoutine(OnComplected);
    }

    public void MenuButtonPressed()
    {
        CloseMenu(() => (LoadingMenu.Instance as LoadingMenu).LoadScene(0));
    }

    public void DisplayMessage(string headerText, string bodyText, Action confrimAction)
    {
        headerDisplay.text = headerText;
        bodyDisplay.text = bodyText;

        onConfirmAction = confrimAction;
    }

    public void Confrim()
    {
        CloseMenu(onConfirmAction);
    }

    void ToggleLoadingContent(bool value)
    {
        content.interactable = value;
        content.blocksRaycasts = value;
    }
}
