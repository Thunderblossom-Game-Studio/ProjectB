using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TimedHazard : MonoBehaviour
{
    [Header("IMPORTANT!")]
    [Tooltip("Is this used with the EntitySpawner.cs script? If so, set this to true.")]
    [SerializeField] private bool _usesEntitySpawner;

    [Header("Hazard Settings")]
    [SerializeField] private GameObject[] _hazards;

    [Tooltip("If true, hazards will be randomly chosen from the array. If false, hazards will be spawned in order.")]
    [SerializeField] private bool _hazardsAreRandom = false;

    [Tooltip("Hazards will spawn above area then fall. Has to have RigidBody component!")]
    [SerializeField] private bool _hazardsFallWithGravity = false;
    [SerializeField] private float _fallHeight = 5.0f;

    [SerializeField] private bool _hazardsRandomScale = false;
    [SerializeField] private float _hazardsScaleMin = 0.7f;
    [SerializeField] private float _hazardsScaleMax = 1.3f;

    [Header("Warning Settings")]
    public GameObject _warningVisual;
    [SerializeField] private float _warningYOffset = 3.0f;

    [Header("System Settings")]
    [SerializeField] private bool _isDebugMode = false;
    [SerializeField] private bool _activeOnStart = true;
    [SerializeField] private bool _destroyOnFinish = true;
   
    [Tooltip("If true, GameObjects will be a child of _Dynamic. If false, they will be a child of this GameObject")]
    [SerializeField] private bool _useDynamic = false;

    [Tooltip("Time in seconds to display warning visual before hazard")]
    [SerializeField] private float _warningTime = 5.0f;

    [Tooltip("Time in seconds that the hazard will be active")]
    [SerializeField] private float _hazardTime = 5.0f;

    [Header("Events")]
    [SerializeField] private UnityEvent _onWarningStart;
    [SerializeField] private UnityEvent _onHazardStart;
    [SerializeField] private UnityEvent _onFinished;

    private GameObject _dynamic;
    private int _hazardIndex = 0;
    private List<GameObject> _hazardInstances = new List<GameObject>();
    private GameObject _warningVisualInstance;
    private float _hazardsScale;

    private void Awake() { Initialize(); }
    
    private void Start() { if (_activeOnStart) StartHazard(); }

    public void StartHazard()
    {
        StartCoroutine(HazardRoutine());
    }

    public void DestroyTimedHazard(int hazardIndex)
    {
        foreach (GameObject hazardInstance in _hazardInstances) { Destroy(hazardInstance); }
        _hazardInstances.Clear();
        Destroy(_warningVisualInstance);
        gameObject.GetComponent<SpawnableObject>().DestroyObject();
    }

    private IEnumerator HazardRoutine()
    {
        int randomHazard = Random.Range(0, _hazards.Length);
        if (_isDebugMode) Debug.Log("Started Warning Time");
        if (_hazardsRandomScale) RandomizeScale(randomHazard);
        ChangeWarningState(_warningVisualInstance, true);
        _onWarningStart.Invoke();

        yield return new WaitForSeconds(_warningTime);
        if (_isDebugMode) Debug.Log("Started Hazard Time");
        _onHazardStart.Invoke();
        ChangeHazardState(true, randomHazard);
        
        yield return new WaitForSeconds(_hazardTime);
        if (_isDebugMode) Debug.Log("Deactivated Hazard");
        _onFinished.Invoke();
        ChangeHazardState(false, randomHazard);
        ChangeWarningState(_warningVisualInstance, false);
        if (_destroyOnFinish) DestroyTimedHazard(randomHazard);
    }

    private void Initialize()
    {
        if (_hazards == null || _hazards.Length < 1)
        {
            Debug.LogError("No hazards assigned!");
            return;
        }

        if (_dynamic == null && _useDynamic == true)
        {
            if (GameObject.Find("_Dynamic") != null)
            {
                _dynamic = GameObject.Find("_Dynamic");
            }
            else
            {
                _dynamic = new GameObject("_Dynamic");
            }
        }
        else
        {
            _dynamic = gameObject;
        }

        foreach (GameObject hazard in _hazards)
        {
            GameObject _hazardInstance = Instantiate(hazard, transform.position, Quaternion.identity, _dynamic.transform);
            UpdateSpawnPosition(_hazardInstance);
            _hazardInstances.Add(_hazardInstance);
        }

        _warningVisualInstance = Instantiate(_warningVisual, transform.position, Quaternion.identity, _dynamic.transform);
        _warningVisualInstance.SetActive(false);
    }

    private void ChangeHazardState(bool isEnabled, int randomHazard)
    {
        GameObject targetHazard;
        if (_hazardsAreRandom) { targetHazard = _hazardInstances[randomHazard]; }
        else
        {
            targetHazard = _hazardInstances[_hazardIndex];
            if (!isEnabled && _hazardIndex < _hazardInstances.Count - 1) { _hazardIndex++; }
            else if (!isEnabled) { _hazardIndex = 0; }
        }
        UpdateSpawnPosition(targetHazard);
        targetHazard.SetActive(isEnabled);
    }

    private void ChangeWarningState(GameObject targetObject, bool isEnabled)
    {
        targetObject.SetActive(isEnabled);
        targetObject.transform.position = new Vector3(transform.position.x, transform.position.y + _warningYOffset, transform.position.z);
        targetObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void UpdateSpawnPosition(GameObject instance)
    {
        if (_hazardsFallWithGravity)
        {
            instance.gameObject.transform.SetParent(transform);
            instance.transform.position = new Vector3(transform.position.x, transform.position.y + _fallHeight, transform.position.z);
            instance.gameObject.transform.SetParent(_dynamic.transform);
        }
        else
        {
            instance.transform.position = transform.position;
        }
    }

    private void RandomizeScale(int index)
    {
        _hazardsScale = Random.Range(_hazardsScaleMin, _hazardsScaleMax);
        _hazardInstances[index].transform.localScale = new Vector3(_hazardsScale, _hazardsScale, _hazardsScale);
        _warningVisualInstance.GetComponent<Light>().spotAngle = 30;
        _warningVisualInstance.GetComponent<Light>().spotAngle = _warningVisualInstance.GetComponent<Light>().spotAngle + (_hazardsScale * 5);
    }


    //create AOEHazard radius (circle)

    //create hazards zone sizes

    //timer OR proximity setting
}
