using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If true, uses range detection. If false, it will use a timer")]
    public bool _usesRangeDetection = true;
    
    //
    public GameObject projectile;
    public Transform target;
    public Transform spawn;
    public float speed;
    public float angle;

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

        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject GO = Instantiate(projectile, spawn.transform.position, Quaternion.identity);
            float calcSpeed = (speed / 16) * Vector2.Distance(spawn.transform.position, target.transform.position);
            StartCoroutine(Curve.TransformCurve(GO, calcSpeed, angle, spawn.transform.position, target.transform.position));
        }
    }

    public void CreateSplatter(GameObject sender)
    {
        Instantiate(_lavaPool, sender.transform.position, Quaternion.identity);
    }
}