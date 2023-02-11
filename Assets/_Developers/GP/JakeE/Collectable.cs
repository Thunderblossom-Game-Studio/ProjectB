using JE.General;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [SerializeField] private LayerMask _collideLayers;

    private void OnTriggerEnter(Collider objectCollider)
    {
        if (!_collideLayers.ContainsLayer(objectCollider.gameObject.layer)) return;
        Collect(objectCollider.gameObject);
        Destroy(gameObject);
    }

    protected abstract void Collect(GameObject collideObject);
}
