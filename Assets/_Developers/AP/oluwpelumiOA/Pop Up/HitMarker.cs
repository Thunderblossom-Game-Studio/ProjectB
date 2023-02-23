using UnityEngine;
using Pelumi.Juicer;

public class HitMarker : PoolObject
{
    [Space(10)]
    [Header("HitMarker")]
    [SerializeField] private JuicerVector3Properties scaleEffect;
    
    private Vector3 spawnPosition;
    
    protected override void OnEnable()
    {
        Effect();
        spawnPosition = transform.position;
    }

    private void Update()
    {
        transform.position = spawnPosition;
    }

    public void Effect()
    {
        StartCoroutine(Juicer.DoVector3(null, Vector3.zero, (pos) => transform.localScale = pos, scaleEffect, DisableObject));
    }
}
