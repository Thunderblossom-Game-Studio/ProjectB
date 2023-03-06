using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DebugMovement : MonoBehaviour
{
    [SerializeField] private float _debugSpeed = 3f;
    [SerializeField] private float _shiftMulti = 2;

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * _debugSpeed * _shiftMulti * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * _debugSpeed * _shiftMulti * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * _debugSpeed * _shiftMulti * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * _debugSpeed * _shiftMulti * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _shiftMulti = 2;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _shiftMulti = 4;
        }
        else
        {
            _shiftMulti = 1;
        }
    }
}