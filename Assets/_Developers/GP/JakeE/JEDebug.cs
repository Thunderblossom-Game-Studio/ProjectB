using System;
using System.Collections;
using System.Collections.Generic;
using JE.DamageSystem;
using UnityEngine;

public class JEDebug : MonoBehaviour
{
    public GameObject _car;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _car.GetComponent<IDamager>().DamageDuration();
        }
    }
}
