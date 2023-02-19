using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pelumi.Juicer;

public class TextPopUp : PoolObject
{
    [Space(10)]
    [Header("TextPopUp")]
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private JuicerVector3Properties scaleEffect;
    
    protected override void OnEnable()
    {
        Effect();
    }

    public void SetText(string text, Color color)
    {
        textMeshPro.text = text;
        textMeshPro.color = color;
    }

    public void Effect()
    {
        StartCoroutine(Juicer.DoVector3(null, transform.position, (pos) => transform.position = pos,

    new JuicerVector3Properties(transform.position + new Vector3(Juicer.GetRange(moveOffset.y), Juicer.GetRange(moveOffset.y), Juicer.GetRange(moveOffset.z)),
    .5f, animationCurveType: AnimationCurveType.EaseInOut), null));
        StartCoroutine(Juicer.DoVector3(null, Vector3.zero, (pos) => transform.localScale = pos, scaleEffect, DisableObject));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
