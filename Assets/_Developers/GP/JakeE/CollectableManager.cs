using System;
using UnityEngine;
using UnityEngine.Pool;

public partial class CollectableManager : Singleton<CollectableManager>
{
    [SerializeField] private Collectable _collectablePrefab;

    public Action GetCollectable(CollectableType collectable)
    {
        switch (collectable)
        {
            case CollectableType.Boost:
                return BoostRefuel;
            case CollectableType.Health:
                return InfiniteBoost;
        }
        return null;
    }

    public Collectable GetCollectablePrefab(CollectableType collectable)
    {
        switch (collectable)
        {
            case CollectableType.Boost:
                return _collectablePrefab;
            case CollectableType.Health:
                return _collectablePrefab;
        }
        return null;
    }

    private void BoostRefuel()
    {
        Debug.Log("boost collectable");
    }

    private void InfiniteBoost()
    {
        Debug.Log("infinite boost collectable");
    }

}

public partial class CollectableManager
{
    [Range(1, 100)] [SerializeField] private int _defaultPoolCapacity = 100;
    [Range(1, 500)] [SerializeField] private int _maxPoolCapacity = 500;
    private ObjectPool<Collectable> _collectablePool;

    private void Start()
    {
        _collectablePool = new ObjectPool<Collectable>(CreateCollectable, OnTake, OnReturned,
            OnCapacity, true, _defaultPoolCapacity, _maxPoolCapacity);
    }


    public void SpawnCollectable(CollectableType collectableType, Vector3 collectableLocation)
    {
        Collectable collectable = _collectablePool.Get();
        collectable.SetType(collectableType);
        collectable.transform.position = collectableLocation;
    }

    public void DestroyCollectable(Collectable collectable)
    {
        _collectablePool.Release(collectable);
    }

    private Collectable CreateCollectable() => Instantiate(_collectablePrefab);
    private void OnReturned(Collectable collectable) => collectable.gameObject.SetActive(false);
    private void OnTake(Collectable collectable) => collectable.gameObject.SetActive(true);
    private void OnCapacity(Collectable collectable) => Destroy(collectable.gameObject);

}

public enum CollectableType
{
    Boost,
    Health
}

