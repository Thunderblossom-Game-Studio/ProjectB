using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarningPrompter : MonoBehaviour
{
    public Transform _player;
    public GameObject _hazardSystem;
    [SerializeField] float _distance = 3.0f;

    private HazzardWarning _volcanoWarningVisual;

    private void Start()
    {
        _volcanoWarningVisual = _hazardSystem.GetComponent<HazzardWarning>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < _distance)
        {
            _volcanoWarningVisual.EnableImageDisplay(0);
        }
        else _volcanoWarningVisual.DisableImageDisplay(0);
    }
}
