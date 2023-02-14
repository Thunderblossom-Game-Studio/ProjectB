using JE.General;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    private EntitySpawner _entitySpawner;
    [SerializeField] private LayerMask _collideLayers;

    private void OnTriggerEnter(Collider objectCollider)
    {
        if (!_collideLayers.ContainsLayer(objectCollider.gameObject.layer)) return;
        Collect(objectCollider.gameObject);
    }
    public void DestroyObject()
    {
        SpawnableObject spawnableObject = GetComponent<SpawnableObject>();
        if (spawnableObject == null) return;
        spawnableObject.DestroyObject();
    }

    protected abstract void Collect(GameObject collideObject);
}
