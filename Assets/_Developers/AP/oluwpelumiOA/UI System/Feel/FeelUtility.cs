using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FeelUtility
{
    public static IEnumerator FadeObjectColourMutiple(Renderer renderer, List<FeelColorProperties> colorProperties, bool loop = false, Action OnFinished = null)
    {
        foreach (FeelColorProperties colourProperty in colorProperties)
        {
            yield return ChangeFadeColor(renderer, colourProperty, OnFinished);
        }

        if (OnFinished != null) OnFinished();

        if (loop) yield return FadeObjectColourMutiple(renderer, colorProperties, loop, OnFinished);
    }

    public static IEnumerator ChangeFadeColor(Renderer renderer, FeelColorProperties colorProperties, Action OnFinished = null)
    {
        float i = 0.0f;
        float rate = 1.0f / colorProperties.duration;
        Color startColor = renderer.material.color;
        Color endColor = colorProperties.color;
        while (i < 1.0f)
        {
            i += Time.unscaledDeltaTime * rate;
            renderer.material.color = Color.Lerp(startColor, endColor, colorProperties.animationCurve.Evaluate(i));
            yield return null;
        }

        if (OnFinished != null) OnFinished();
    }

    public static IEnumerator RotateObjectMultiple(Transform thisTransform, List<FeelRotateProperties> rotateProperties, bool loop = false, Action OnFinished = null)
    {
        foreach (FeelRotateProperties rotateProperty in rotateProperties)
        {
            yield return RotateObject(thisTransform, rotateProperty, OnFinished);
        }

        if (OnFinished != null) OnFinished();

        if(loop)yield return RotateObjectMultiple(thisTransform, rotateProperties, loop, OnFinished);
    }

    public static IEnumerator RotateObject(Transform thisTransform, FeelRotateProperties rotateProperties, Action OnFinished = null)
    {
        float i = 0.0f;
        float rate = 1.0f / rotateProperties.duration;
        Vector3 startRotation = thisTransform.eulerAngles;
        while (i < 1.0f)
        {
            i += Time.unscaledDeltaTime * rate;
            thisTransform.eulerAngles = Vector3.Lerp(startRotation, rotateProperties.rotation, rotateProperties.animationCurve.Evaluate(i));
            yield return null;
        }

        if (OnFinished != null) OnFinished();
    }

    public static IEnumerator MultipleFadeVector3(Action OnBegin, Transform thisTransform, List<FeelVector3Properties> feelVector3Properties, bool loop = false, Action OnFinished = null)
    {
        OnBegin?.Invoke();
        
        foreach (FeelVector3Properties feelVector3Property in feelVector3Properties)
        {
            yield return FadeVector3(null, thisTransform.position, (value) => thisTransform.position = value, feelVector3Property, null);
            if (OnFinished != null) OnFinished();
            if (loop) yield return MultipleFadeVector3(OnBegin, thisTransform, feelVector3Properties, loop, OnFinished);
        }
    }

    public static IEnumerator FadeVector3(Action OnBegin, Vector3 startingValue, Action<Vector3> valueToModify, FeelVector3Properties feelVector3Properties, Action OnFinished = null)
    {
        OnBegin?.Invoke();
        float i = 0.0f;
        float rate = 1.0f / feelVector3Properties.duration;
        Vector3 startValue = startingValue;
        while (i < 1.0f)
        {
            i += Time.unscaledDeltaTime * rate;
            valueToModify.Invoke(Vector3.Lerp(startValue, feelVector3Properties.destination, feelVector3Properties.animationCurve.Evaluate(i)));
            yield return null;
        }

        if (OnFinished != null) OnFinished();
    }

    public static IEnumerator FadeFloat(Action OnBegin, float startingValue, Action<float> valueToModify, FeelFloatProperties feelFloatProperties,  Action OnFinished = null)
    {
        OnBegin?.Invoke();
        
        float i = 0.0f;
        float rate = 1.0f / feelFloatProperties.duration;
        float startValue = startingValue;
        while (i < 1.0f)
        {
            i += Time.unscaledDeltaTime * rate;
            valueToModify.Invoke(Mathf.Lerp(startValue, feelFloatProperties.destination, feelFloatProperties.animationCurve.Evaluate(i)));
            yield return null;
        }

       OnFinished?.Invoke();
    }


    public static AnimationCurve GetAnimationCurve (AnimationCurveType animationCurveType)
    {
        switch (animationCurveType)
        {
            case AnimationCurveType.Linear: return AnimationCurve.Linear(0, 0, 1, 1);
            case AnimationCurveType.EaseInOut:  return AnimationCurve.EaseInOut(0, 0, 1, 1);
            case AnimationCurveType.Constant: return AnimationCurve.Constant(0, 1, 1);
        }
        return AnimationCurve.Linear(0, 0, 1, 1);
    }
}

[Serializable]
public class RotateStat
{
    public bool canRotate;
}

[Serializable]
public class FeelMoveProperties
{
    public Vector3 position;
    public float duration;
    public AnimationCurve animationCurve;
    public bool relative;

    public FeelMoveProperties(Vector3 position, float duration, bool relative = false, AnimationCurve animationCurve = null, AnimationCurveType animationCurveType = AnimationCurveType.Linear )
    {
        this.position = position;
        this.duration = duration;
        this.animationCurve = animationCurve == null ? FeelUtility.GetAnimationCurve(animationCurveType) : animationCurve;
        this.relative = relative;
    }
}

[Serializable]
public class FeelScaleProperties
{
    public Vector3 scale;
    public float duration;
    public AnimationCurve animationCurve;
    public bool relative;

    public FeelScaleProperties(Vector3 scale, float duration, bool relative = false, AnimationCurve animationCurve = null, AnimationCurveType animationCurveType = AnimationCurveType.Linear)
    {
        this.scale = scale;
        this.duration = duration;
        this.animationCurve = animationCurve == null ? FeelUtility.GetAnimationCurve(animationCurveType) : animationCurve;
        this.relative = relative;
    }
}

[Serializable]
public class FeelColorProperties
{
    public Color color;
    public float duration;
    public AnimationCurve animationCurve;

    public FeelColorProperties(Color color, float duration, AnimationCurve animationCurve = null, AnimationCurveType animationCurveType = AnimationCurveType.Linear)
    {
        this.color = color;
        this.duration = duration;
        this.animationCurve = animationCurve == null ? FeelUtility.GetAnimationCurve(animationCurveType) : animationCurve;
    }
}

[Serializable]
public class FeelRotateProperties
{
    public Vector3 rotation;
    public float duration;
    public AnimationCurve animationCurve;

    public FeelRotateProperties(Vector3 rotation, float duration, AnimationCurve animationCurve = null, AnimationCurveType animationCurveType = AnimationCurveType.Linear)
    {
        this.rotation = rotation;
        this.duration = duration;
        this.animationCurve = animationCurve == null ? FeelUtility.GetAnimationCurve(animationCurveType) : animationCurve;
    }
}

[Serializable]
public class FeelFloatProperties
{
    public float destination;
    public float duration;
    public AnimationCurve animationCurve;
    
    public FeelFloatProperties(float destination, float duration, AnimationCurve animationCurve = null, AnimationCurveType animationCurveType = AnimationCurveType.Linear)
    {
        this.destination = destination;
        this.duration = duration;
        this.animationCurve = animationCurve == null ? FeelUtility.GetAnimationCurve(animationCurveType) : animationCurve;
    }
}

[Serializable]
public class FeelVector3Properties
{
    public Vector3 destination;
    public float duration;
    public AnimationCurve animationCurve;

    public FeelVector3Properties(Vector3 destination, float duration, AnimationCurve animationCurve = null, AnimationCurveType animationCurveType = AnimationCurveType.Linear)
    {
        this.destination = destination;
        this.duration = duration;
        this.animationCurve = animationCurve == null ? FeelUtility.GetAnimationCurve(animationCurveType) : animationCurve;
    }
}

public enum AnimationCurveType
{
    Linear,
    EaseInOut,
    Constant
}