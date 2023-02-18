using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabVariableManager : MonoBehaviour
{
    public static bool TrafficPanic = false;
    public bool DisplayTrafficPanic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayTrafficPanic = TrafficPanic;
    }
}
