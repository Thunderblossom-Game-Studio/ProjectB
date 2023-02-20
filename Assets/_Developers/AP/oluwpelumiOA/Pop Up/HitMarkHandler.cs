using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class HitMarkHandler : MonoBehaviour
{
    [SerializeField] private RectTransform spawnPos;
    [SerializeField] private GameObject hitMarkPrefab;

    [SerializeField] private GameEvent onHit;

    private ObjectPool<GameObject> hitMarkPool;

    private void OnEnable()
    {
        onHit.Register(TriggerHitmark);
    }

    private void Start()
    {
        hitMarkPool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDismiss, true);
    }

    public GameObject OnCreate()
    {
        GameObject gameObject = Instantiate(hitMarkPrefab);
        if (gameObject.TryGetComponent(out PoolObject poolerObject)) poolerObject.AssignPooler(hitMarkPool);
        return gameObject;
    }

    public void OnGet(GameObject gameObjectToGet)
    {
        gameObjectToGet.GetComponent<RectTransform>().SetParent(spawnPos);
    }

    public void OnRelease(GameObject gameObjectToRelease)
    {
        gameObjectToRelease.SetActive(false);
    }

    public void OnDismiss(GameObject gameObjectToDestroy)
    {
        Destroy(gameObjectToDestroy);
    }

    public void TriggerHitmark(Component arg1, object arg2)
    {
        ShotHitmark((HitMarkInfo)arg2);
    }

    public void ShotHitmark(HitMarkInfo hitMarkInfo)
    {
        GameObject hitmark = hitMarkPool.Get();
        Image hitMarker = hitmark.GetComponent<Image>();
        hitMarker.rectTransform.position = Camera.main.WorldToScreenPoint(hitMarkInfo.spawnPos);
        hitMarker.color = hitMarkInfo.color;
        hitMarker.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        onHit.Unregister(TriggerHitmark);
    }
}

[System.Serializable]
public struct HitMarkInfo
{
    public Color color;
    public Vector3 spawnPos;

    public HitMarkInfo(Color color, Vector3 spawnPos)
    {
        this.color = color;
        this.spawnPos = spawnPos;
    }
}