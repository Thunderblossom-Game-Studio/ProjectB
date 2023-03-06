using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlimpWarningPrompter : MonoBehaviour
{
    public Transform _player;
    public GameObject _hazardSystem;
    [SerializeField] float _distance = 3.0f;

    private HazzardWarning _BlimpWarningVisual;

    private Vector3 underblimp;
    void Start()
    {
        _BlimpWarningVisual = _hazardSystem.GetComponent<HazzardWarning>();
    }

    
    void Update()
    {
        underblimp = transform.position;

        underblimp.y = _player.transform.position.y;

        if (Vector3.Distance(underblimp, _player.transform.position) < _distance)
        {
            _BlimpWarningVisual.EnableImageDisplay(1);
        }
        else _BlimpWarningVisual.DisableImageDisplay(1);
    }
}
