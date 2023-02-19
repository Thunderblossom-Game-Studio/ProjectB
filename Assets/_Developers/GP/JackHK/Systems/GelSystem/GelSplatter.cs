using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GelSplatter : MonoBehaviour
{
    public GelSystem _gelSystem;
    [SerializeField] private bool _usesGelSystem = true;

    [Tooltip("Splatter life before destroyed")]
    [SerializeField] private int _splatterLife = 3;

    [SerializeField] private bool _usesLifeTimer = false;

    [Tooltip("Splatter life timer in seconds before splatter is destroyed")]
    [SerializeField] private float _splatterLifeTime = 10.0f;
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private string _enemyTag = "Enemy";

    private int _life;
    private bool _timerIsRunning = false;

    [Header("Events")]
    [SerializeField] private UnityEvent _onTouched;
    [SerializeField] private UnityEvent _onCleanedUp;

    private void Awake()
    {
        if (_gelSystem == null && _usesGelSystem) { _gelSystem = FindObjectOfType<GelSystem>(); }
    }

    private void Start()
    {
        if (_usesGelSystem) { _life = _gelSystem._splatterLife; }
        else { _life = _splatterLife; }
    }

    private void Update()
    {
        if (_usesLifeTimer)
        {
            if (_timerIsRunning) { return; }
            else { StartCoroutine(LifeTimer()); }
        }
    }

    private void OnTriggerEnter(Collider target)
    {
        //on character touched
        if (_usesGelSystem)
        {
            if (target.gameObject.tag == _gelSystem._playerTagName || target.gameObject.tag == _gelSystem._enemyTagName) OnTouched();
        }
        else
        {
            if (target.gameObject.tag == _playerTag || target.gameObject.tag == _enemyTag) OnTouched();
        }
    }

    private IEnumerator LifeTimer()
    {
        _timerIsRunning = true;
        yield return new WaitForSeconds(_splatterLifeTime);
        DestroySplatter();
    }


    private void OnTouched()
    {
        _onTouched.Invoke();
        gameObject.transform.localScale = gameObject.transform.localScale / 0.33f;
        _life -= 1;

        if (_life <= 0) { DestroySplatter(); }
    }

    public void DestroySplatter()
    {
        _onCleanedUp.Invoke();
        Destroy(gameObject);
    }
}
