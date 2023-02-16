using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolObject : MonoBehaviour
{
    [Header("PoolObject")]
    [SerializeField] protected bool forceDisable;
    [SerializeField] protected float destoryTime;
    [SerializeField] protected bool resetScale;
    [SerializeField] protected bool resetRotation;

    protected Vector3 startScale;
    protected Quaternion startRotation;
    protected ObjectPool<GameObject> pool;

    protected virtual void OnEnable()
    {
        if (forceDisable) Invoke(nameof(DisableObject), destoryTime);
    }

    protected void Start()
    {
        startScale = transform.localScale;
        startRotation = transform.localRotation;
    }

    public virtual void AssignPooler(ObjectPool<GameObject> objectPool)
    {
        pool = objectPool;
    }

    public virtual void DisableObject()
    {
        if (resetScale) transform.localScale = startScale;
        if (resetRotation) transform.rotation = startRotation;
        pool.Release(gameObject);
    }

    private void OnDisable() { if (forceDisable) CancelInvoke(nameof(DisableObject)); }
}
