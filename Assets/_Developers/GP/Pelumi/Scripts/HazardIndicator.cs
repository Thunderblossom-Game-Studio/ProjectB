using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pelumi.Juicer;
using UnityEngine.UI;

public class HazardIndicator : MonoBehaviour
{
    public static HazardIndicator Instance;

    public enum IndicatorType { Volcano, Blimp}

    [SerializeField] private Image volcanoIndicator;
    [SerializeField] private Image blimpIndicator;

    [SerializeField] private List<JuicerVector3Properties> _scaleEffect;

    private Coroutine _VolcanoIndicatorRoutine;
    private Coroutine _blimpIndicatorRoutine;

    private void Awake()
    {
        Instance = this;
    }

    public void ActivateIndicator(IndicatorType indicatorType)
    {
        ResetRoutine(indicatorType);

        switch (indicatorType)
        {
            case IndicatorType.Volcano:
                volcanoIndicator.enabled = true;
                _VolcanoIndicatorRoutine = StartCoroutine(Juicer.DoMultipleVector3(null, Vector3.zero, (pos) => volcanoIndicator.transform.localScale = pos, _scaleEffect, 0, true));
                break;
            case IndicatorType.Blimp:
                 blimpIndicator.enabled = true;
                _blimpIndicatorRoutine = StartCoroutine(Juicer.DoMultipleVector3(null, Vector3.zero, (pos) => blimpIndicator.transform.localScale = pos, _scaleEffect, 0, true));
                break;
        }
    }

    public void DeActivateIndicator(IndicatorType indicatorType)
    {
        switch (indicatorType)
        {
            case IndicatorType.Volcano:
                volcanoIndicator.enabled = false;
                StopCoroutine(_VolcanoIndicatorRoutine);
                break;
            case IndicatorType.Blimp:
                blimpIndicator.enabled = false;
                StopCoroutine(_blimpIndicatorRoutine);
                break;
        }
    }


    public void ResetRoutine(IndicatorType indicatorType)
    {
        switch (indicatorType)
        {
            case IndicatorType.Volcano:
                 if(_VolcanoIndicatorRoutine != null) StopCoroutine(_VolcanoIndicatorRoutine);
                break;
            case IndicatorType.Blimp:
                if (_blimpIndicatorRoutine != null) StopCoroutine(_blimpIndicatorRoutine);
                break;
        }
    }
}
