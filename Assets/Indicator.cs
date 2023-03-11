using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    Renderer render;

    public float timeRemaining = 30;
    public float scale = 3;

    private void Start()
    {
        render = gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            StartCoroutine(FadeColor(render, Color.red, new Color(1, 0, 0, 0), 2f));
            StartCoroutine(Scale(render, scale, 2f));
            timeRemaining = 30;
        }
    }

    private IEnumerator FadeColor(Renderer objectRenderer, Color startColor, Color endColor, float fadeDuration)
    {
        float currentDuration = 0;
        while (currentDuration < fadeDuration)
        {
            currentDuration += Time.deltaTime;
            objectRenderer.material.color = Color.Lerp
                (startColor, endColor, currentDuration / fadeDuration);
            yield return null;
        }
    }

    private IEnumerator Scale(Renderer objectRenderer, float scale, float fadeDuration)
    {
        float currentDuration = 0;
        while (currentDuration < fadeDuration)
        {
            currentDuration += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale * scale, Time.deltaTime * fadeDuration);
            yield return null;
        }
    }
}
