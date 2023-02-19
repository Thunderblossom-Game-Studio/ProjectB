using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Pelumi.Juicer;

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

        scaleRoutine = StartCoroutine(Juicer.DoVector3(null, transform.localScale, (pos) => transform.localScale = pos, new JuicerVector3Properties(new Vector3(1, 1, 1), .1f, animationCurveType: AnimationCurveType.EaseInOut), null));
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        OnAnyButtonHovered?.Invoke(this, EventArgs.Empty);
        base.OnPointerEnter(eventData);
        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(Juicer.DoVector3(null, transform.localScale, (pos) => transform.localScale = pos, new JuicerVector3Properties(new Vector3(1.2f, 1.2f, 1.2f), .1f, animationCurveType: AnimationCurveType.EaseInOut), null));
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(Juicer.DoVector3(null, transform.localScale, (pos) => transform.localScale = pos, new JuicerVector3Properties(new Vector3(1, 1, 1), .1f, animationCurveType: AnimationCurveType.EaseInOut), null));
    }

    public override void OnSelect(BaseEventData eventData)
    {
        OnAnyButtonHovered?.Invoke(this, EventArgs.Empty);
        base.OnSelect(eventData);
        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(Juicer.DoVector3(null, transform.localScale, (pos) => transform.localScale = pos, new JuicerVector3Properties(new Vector3(1.2f, 1.2f, 1.2f), .1f, animationCurveType: AnimationCurveType.EaseInOut), null));
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(Juicer.DoVector3(null, transform.localScale, (pos) => transform.localScale = pos, new JuicerVector3Properties(new Vector3(1f, 1f, 1f), .1f, animationCurveType: AnimationCurveType.EaseInOut), null));
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
