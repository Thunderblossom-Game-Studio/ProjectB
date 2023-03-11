using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    Renderer render;

    public Vector3 scale = new Vector3(2,1,2);
    public float timeToLand;

    private void Start()
    {
        render = gameObject.GetComponent<Renderer>();
    }

    public void UpdateStuff(float _timeToLand)
    {
        timeToLand = _timeToLand;
        StartCoroutine(FadeColor(render, Color.red, new Color(1, 0, 0, 0), 2f));
        StartCoroutine(Scale(scale));
    }

        private IEnumerator FadeColor(Renderer objectRenderer, Color startColor, Color endColor, float fadeDuration)
    {
        float currentDuration = 0;
        while (currentDuration < fadeDuration)
        {
            currentDuration += Time.deltaTime;
            if (objectRenderer == null) break;
            objectRenderer.material.color = Color.Lerp
                (startColor, endColor, currentDuration / fadeDuration);
            yield return null;
        }
    }

    private IEnumerator Scale(Vector3 scale)
    {
        float startTime = Time.time; // Store the start time of the coroutine
        while (Time.time < startTime + timeToLand) // Loop until the duration has passed
        {
            float t = (Time.time - startTime) / timeToLand; // Calculate the current time fraction
            transform.localScale = Vector3.Lerp(transform.localScale, scale, t); // Interpolate between the initial scale and the target scale
            yield return null; // Wait for the next frame
        }
        transform.localScale = scale; // Set the final scale to the target scale
    }
}
