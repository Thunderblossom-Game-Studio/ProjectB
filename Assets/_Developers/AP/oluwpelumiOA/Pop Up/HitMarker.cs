using UnityEngine;

public class HitMarker : PoolObject
{
    [Space(10)]
    [Header("HitMarker")]
    [SerializeField] private FeelVector3Properties scaleEffect;
    
    protected override void OnEnable()
    {
        Effect();
    }

    public void Effect()
    {
        StartCoroutine(FeelUtility.FadeVector3(null, Vector3.zero, (pos) => transform.localScale = pos, scaleEffect, DisableObject));
    }
}
