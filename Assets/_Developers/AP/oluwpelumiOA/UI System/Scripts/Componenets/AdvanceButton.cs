using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class AdvanceButton : Button
{
    public static event EventHandler OnAnyButtonHovered;
    public static event EventHandler OnAnyButtonClicked;

    [SerializeField] private UnityEvent onMouseOver;
    [SerializeField] private UnityEvent onMouseExit;

    private Coroutine scaleRoutine;

    protected override void OnEnable()
    {
        interactable = true;
        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(FeelUtility.ScaleObject(transform, new FeelScaleProperties(new Vector3(1, 1, 1), .1f, animationCurveType: AnimationCurveType.EaseInOut)));
    }
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        OnAnyButtonHovered?.Invoke(this, EventArgs.Empty);
        base.OnPointerEnter(eventData);
        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(FeelUtility.ScaleObject(transform, new FeelScaleProperties(new Vector3(1.2f, 1.2f, 1.2f), .1f, animationCurveType: AnimationCurveType.EaseInOut)));
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(FeelUtility.ScaleObject(transform, new FeelScaleProperties(new Vector3(1, 1, 1), .1f, animationCurveType: AnimationCurveType.EaseInOut)));
    }

    public override void OnSelect(BaseEventData eventData)
    {
        OnAnyButtonHovered?.Invoke(this, EventArgs.Empty);
        base.OnSelect(eventData);
        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(FeelUtility.ScaleObject(transform, new FeelScaleProperties(new Vector3(1.2f, 1.2f, 1.2f), .1f, animationCurveType: AnimationCurveType.EaseInOut)));
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(FeelUtility.ScaleObject(transform, new FeelScaleProperties(new Vector3(1, 1, 1), .1f, animationCurveType: AnimationCurveType.EaseInOut)));
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        if(interactable) OnAnyButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (interactable) OnAnyButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
