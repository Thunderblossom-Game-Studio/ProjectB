using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If true, uses range detection. If false, it will use a timer")]
    public bool _usesRangeDetection = true;

    [Space(10)]
    [SerializeField] private string PlayerTagName = "Player";
    [SerializeField] private EntityAutoSpawner _spawner;
    [SerializeField] private GameObject _lavaPool;

    private bool _playerInRange = false;

    private void OnTriggerStay(Collider other)
    {
        if (_usesRangeDetection)
        {
            if (other.tag == PlayerTagName) { _playerInRange = true; }
            else { _playerInRange = false; }
            if (other == null) { _playerInRange = false; }
        }

    }

    private void Update()
    {
        if (_usesRangeDetection)
        {
            if (_playerInRange) _spawner.enabled = true;
            else _spawner.enabled = false;
        }
    }

    public void CreateSplatter(GameObject sender)
    {
        Instantiate(_lavaPool, sender.transform.position, Quaternion.identity);
    }
}