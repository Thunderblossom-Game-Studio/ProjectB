using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DebugMovement : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    [SerializeField] private GameObject debug_driveSpeed;
    [SerializeField] private GameObject debug_boostSpeed;

    [SerializeField] private float _debugSpeed = 3f;
    [SerializeField] private float _shiftMulti = 2;

    public void SetDriveSpeedDebug()
    {
        _debugSpeed = float.Parse(debug_driveSpeed.GetComponent<InputField>().text);
    }

    void Update()
    {
        if (VechicleResources.Instance._resources[0]._amount <= 0)
        {
            _arrow.SetActive(false);
            return;
        }
        if (Input.anyKey)
        {
            _arrow.SetActive(true);
            VechicleResources.Instance.BurnResource("Fuel", VechicleResources.Instance._burnRate);
        }
        else
        {
            _arrow.SetActive(false);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _arrow.transform.eulerAngles = new Vector3(0, -90, 0);
            transform.position += Vector3.right * _debugSpeed * _shiftMulti * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _arrow.transform.eulerAngles = new Vector3(0, 90, 0);
            transform.position += Vector3.left * _debugSpeed * _shiftMulti * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            _arrow.transform.eulerAngles = new Vector3(0, 180, 0);
            transform.position += Vector3.forward * _debugSpeed * _shiftMulti * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            _arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            transform.position += Vector3.back * _debugSpeed * _shiftMulti * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            VechicleResources.Instance.BurnResource("Fuel", VechicleResources.Instance._burnRate * 3);
            _shiftMulti = 2;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            VechicleResources.Instance.BurnResource("Boost", VechicleResources.Instance._burnRate * 6);
            _shiftMulti = 4;
        }
        else
        {
            _shiftMulti = 1;
        }
    }
}
