using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField] private RectTransform recttransform;
    [Header("Credits Scroll Settings")]
    [Tooltip("The Ypos of the credits text")]
    public float endpos = 0f;
    [Tooltip("The scroll speed")]
    public float scrollspeed = 0f;
    
    private void Start()
    {
        recttransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        recttransform.anchoredPosition += Vector2.up * scrollspeed * Time.deltaTime;

        if (recttransform.anchoredPosition.y > endpos)
        {
            recttransform.anchoredPosition = new Vector2(recttransform.anchoredPosition.x, endpos);
        }
    }
}
