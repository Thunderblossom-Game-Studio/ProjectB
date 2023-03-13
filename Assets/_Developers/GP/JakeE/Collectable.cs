using JE.General;
using UnityEngine;
using UnityEngine.Events;

public abstract class Collectable : MonoBehaviour
{
    private EntitySpawner _entitySpawner;
    [SerializeField] protected UnityEvent _onCollect;

    private void OnTriggerEnter(Collider objectCollider)
    {
        Collect(objectCollider.gameObject);
    }
    protected void DestroyObject()
    {
        SpawnableObject spawnableObject = GetComponent<SpawnableObject>();
        if (spawnableObject == null) { Destroy(gameObject); return; }
        spawnableObject.DestroyObject();
    }

    protected abstract void Collect(GameObject collideObject);
}
