using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Serializing;
using Unity.VisualScripting;

public interface IVehicleComponent
{
    void GetFullState(Writer writer);
    void SetFullState(Reader reader);
    void Simulate();
}
