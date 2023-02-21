using UnityEngine;
using Pelumi.Juicer;

public class HitMarker : PoolObject
{
    [Space(10)]
    [Header("HitMarker")]
    [SerializeField] private JuicerVector3Properties scaleEffect;
    
    protected override void OnEnable()
    {
        Effect();
    }

    public void Effect()
    {
        StartCoroutine(Juicer.DoVector3(null, Vector3.zero, (pos) => transform.localScale = pos, scaleEffect, DisableObject));
    }
}
