using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Pelumi.Juicer;

namespace Pelumi.Juicer
{
    public static class Juicer
    {
        public static IEnumerator DoMutipleChangeColor(Renderer renderer, List<JuicerColorProperties> colorProperties, bool loop = false, Action OnFinished = null)
        {
            foreach (JuicerColorProperties colourProperty in colorProperties)
            {
                yield return DoChangeColor(renderer, colourProperty, OnFinished);
            }

            if (OnFinished != null) OnFinished();

            if (loop) yield return DoMutipleChangeColor(renderer, colorProperties, loop, OnFinished);
        }

        public static IEnumerator DoChangeColor(Renderer renderer, JuicerColorProperties colorProperties, Action OnFinished = null)
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

        public static IEnumerator DoMultipleVector3(Action OnBegin, Vector3 startingValue, Action<Vector3> valueToModify, List<JuicerVector3Properties> feelVector3Properties, float delay = 0, bool loop = false, Action OnEachFinished = null, Action OnRoundFinished = null)
        {
            OnBegin?.Invoke();
            Vector3 starValue = startingValue;
            for (int i = 0; i < feelVector3Properties.Count; i++)
            {
                yield return DoVector3(null, starValue, valueToModify, feelVector3Properties[i], null);
                starValue = feelVector3Properties[i].destination;
                OnEachFinished?.Invoke();
                yield return new WaitForSecondsRealtime(delay);
            }

            if (OnRoundFinished != null) OnRoundFinished();
            if (loop) yield return DoMultipleVector3(OnBegin, starValue, valueToModify, feelVector3Properties, delay, loop, OnEachFinished, OnRoundFinished);
        }


        public static IEnumerator DoVector3(Action OnBegin, Vector3 startingValue, Action<Vector3> valueToModify, JuicerVector3Properties feelVector3Properties, Action OnFinished = null)
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
        
        public static IEnumerator DoFloat(Action OnBegin, float startingValue, Action<float> valueToModify, JuicerFloatProperties feelFloatProperties, Action OnFinished = null)
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

        public static float GetRange(float value)
        {
            return Random.Range(-value, value);
        }


        public static AnimationCurve GetAnimationCurve(AnimationCurveType animationCurveType)
        {
            switch (animationCurveType)
            {
                case AnimationCurveType.Linear: return AnimationCurve.Linear(0, 0, 1, 1);
                case AnimationCurveType.EaseInOut: return AnimationCurve.EaseInOut(0, 0, 1, 1);
                case AnimationCurveType.Constant: return AnimationCurve.Constant(0, 1, 1);
            }
            return AnimationCurve.Linear(0, 0, 1, 1);
        }
    }
}



[Serializable]
public class RotateStat
{
    public bool canRotate;
}

[Serializable]
public class JuicerColorProperties
{
    public Color color;
    public float duration;
    public AnimationCurve animationCurve;

    public JuicerColorProperties(Color color, float duration, AnimationCurve animationCurve = null, AnimationCurveType animationCurveType = AnimationCurveType.Linear)
    {
        this.color = color;
        this.duration = duration;
        this.animationCurve = animationCurve == null ? Juicer.GetAnimationCurve(animationCurveType) : animationCurve;
    }
}

[Serializable]
public class JuicerFloatProperties
{
    public float destination;
    public float duration;
    public AnimationCurve animationCurve;
    
    public JuicerFloatProperties(float destination, float duration, AnimationCurve animationCurve = null, AnimationCurveType animationCurveType = AnimationCurveType.Linear)
    {
        this.destination = destination;
        this.duration = duration;
        this.animationCurve = animationCurve == null ? Juicer.GetAnimationCurve(animationCurveType) : animationCurve;
    }
}

[Serializable]
public class JuicerVector3Properties
{
    public Vector3 destination;
    public float duration;
    public AnimationCurve animationCurve;

    public JuicerVector3Properties(Vector3 destination, float duration, AnimationCurve animationCurve = null, AnimationCurveType animationCurveType = AnimationCurveType.Linear)
    {
        this.destination = destination;
        this.duration = duration;
        this.animationCurve = animationCurve == null ? Juicer.GetAnimationCurve(animationCurveType) : animationCurve;
    }
}

public enum AnimationCurveType
{
    Linear,
    EaseInOut,
    Constant
}