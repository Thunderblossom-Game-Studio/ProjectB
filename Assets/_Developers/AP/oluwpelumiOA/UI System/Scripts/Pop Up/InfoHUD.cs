using Pelumi.Juicer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoHUD : MonoBehaviour
{
    [SerializeField] private CanvasGroup content;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private GameEvent infoEvent;

    private void OnEnable()
    {
        infoEvent.Register(InfoEvent);
    }

    private void OnDisable()
    {
        infoEvent.Unregister(InfoEvent);
    }

    private void InfoEvent(Component arg1, object arg2)
    {
        InfoHUDData infoHUD = (InfoHUDData)arg2;
        if (infoHUD.Enable)
        {
            displayText.text = infoHUD.Message;
            StartCoroutine(OpenMenuRoutine());
        }
        else StopCoroutine(CloseMenuRoutine());
    }

    public IEnumerator OpenMenuRoutine()
    {
        StartCoroutine(Juicer.DoVector3(null, Vector3.zero, (pos) => content.transform.localScale = pos, new JuicerVector3Properties(Vector3.one, .2f, animationCurveType: AnimationCurveType.EaseInOut), null));
        yield return Juicer.DoFloat(null, 0, (pos) => content.alpha = pos, new JuicerFloatProperties(1, .2f, animationCurveType: AnimationCurveType.EaseInOut), null);
    }

    public IEnumerator CloseMenuRoutine()
    {
        StartCoroutine(Juicer.DoVector3(null, content.transform.localScale, (pos) => content.transform.localScale = pos, new JuicerVector3Properties(Vector3.zero, .2f, animationCurveType: AnimationCurveType.EaseInOut), null));
        yield return Juicer.DoFloat(null, content.alpha, (pos) => content.alpha = pos, new JuicerFloatProperties(0, .2f, animationCurveType: AnimationCurveType.EaseInOut), null);
    }

    public IEnumerator LifeTimeRoutine()
    {
        yield return new  WaitForSeconds(0);
    }
}

public struct InfoHUDData
{
    public bool Enable;
    public string Message;
    public string Duration;
}
