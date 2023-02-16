using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUp : PoolObject
{
    [Space(10)]
    [Header("DamagePopUp")]
    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private FeelVector3Properties scaleEffect;
    
    protected override void OnEnable()
    {
        Effect();
    }
    
    public void Effect()
    {
        StartCoroutine(FeelUtility.FadeVector3(null, transform.position, (pos) => transform.position = pos,
    new FeelVector3Properties(new Vector3(FeelUtility.GetRange(moveOffset.y), FeelUtility.GetRange(moveOffset.y), FeelUtility.GetRange(moveOffset.z)),
    .1f, animationCurveType: AnimationCurveType.EaseInOut), null));
        StartCoroutine(FeelUtility.FadeVector3(null, Vector3.zero, (pos) => transform.localScale = pos, scaleEffect, DisableObject));
    }

    public override void DisableObject()
    {
        StopAllCoroutines();
        base.DisableObject();
    }
}
