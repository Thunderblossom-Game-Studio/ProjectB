using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : NetworkBehaviour, IVehicleComponent
{


    private void Start()
    {
        
    }

    void IVehicleComponent.GetFullState(FishNet.Serializing.Writer writer)
    {

    }   
    
    void IVehicleComponent.SetFullState(FishNet.Serializing.Reader reader)
    {

    }

    void IVehicleComponent.Simulate()
    {

    }
}
