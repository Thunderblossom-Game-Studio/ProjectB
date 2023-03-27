using System;
using System.Collections;
using System.Collections.Generic;
using JE.DamageSystem;
using UnityEngine;

public class DebugHealthSystem : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            GetComponent<IDamageable>().ReduceHealth(50);
        }
    }
}
