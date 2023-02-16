using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using UnityEngine.Timeline;

public class HitMarkHandler : MonoBehaviour
{
    [SerializeField] private GameObject hitMarkPrefab;

    [SerializeField] private GameEvent onHit;

    private ObjectPool<GameObject> hitMarkPool;

    private void OnEnable()
    {
        onHit.Register(TriggerHitmark);
    }

    private void Start()
    {
        hitMarkPool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy, true);
    }

    public GameObject OnCreate()
    {
        GameObject gameObject = Instantiate(hitMarkPrefab);
        if (gameObject.TryGetComponent(out PoolObject poolerObject)) poolerObject.AssignPooler(hitMarkPool);
        return gameObject;
    }

    public void OnGet(GameObject gameObjectToGet)
    {
        gameObjectToGet.transform.SetParent(transform);
    }

    public void OnRelease(GameObject gameObjectToRelease)
    {
        gameObjectToRelease.SetActive(false);
    }

    public void OnDestroy(GameObject gameObjectToDestroy)
    {
        Destroy(gameObjectToDestroy);
    }

    public void TriggerHitmark(Component arg1, object arg2)
    {
        ShotHitmark(Color.red, (Vector3)arg2);
    }

    public void ShotHitmark(Color newColor, Vector3 spawnPos)
    {
        GameObject hitmark = hitMarkPool.Get();
        Image hitMarker = hitmark.GetComponent<Image>();
        hitMarker.rectTransform.position = Camera.main.WorldToScreenPoint(spawnPos);
        hitMarker.color = newColor;
        hitMarker.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        onHit.Unregister(TriggerHitmark);
    }
}
