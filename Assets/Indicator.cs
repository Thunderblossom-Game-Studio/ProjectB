using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    Renderer render;

    public Vector3 scale = new Vector3(2, 1, 2);
    public float fadeDuration = 1;
    public float fadeAmount = 0.1f;

    float timeToLand;
    float currentDuration = 0;

    private void Start()
    {
        render = gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        FadeColor(render);
        currentDuration += Time.deltaTime;
    }

    public void UpdateStuff(float _timeToLand)
    {
        timeToLand = _timeToLand;
        StartCoroutine(Scale(scale));
    }

    private void FadeColor(Renderer objectRenderer)
    {
        if (currentDuration > fadeDuration)
        {
            currentDuration = 0;
            if (objectRenderer != null)
                if (objectRenderer.material.color.a <= 0) return;
            objectRenderer.material.color -= new Color(0, 0, 0, fadeAmount);
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
