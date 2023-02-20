using FishNet.Serializing;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NetCarController : MonoBehaviour
{
    public List<IVehicleComponent> carComponents = new List<IVehicleComponent>();


    public void RegisterComponent(IVehicleComponent component)
    {
        carComponents.Add(component);
    }

    public void SimulateAllComponents()
    {
        foreach (IVehicleComponent component in carComponents)
        {
            component.Simulate();
        }
    }

    public void GetAllStates(Writer writer)
    {
        foreach (IVehicleComponent component in carComponents)
        {
            component.GetFullState(writer);
        }
    }

    public void SetAllStates(Reader reader)
    {
        foreach (IVehicleComponent component in carComponents)
        {
            component.SetFullState(reader);
        }
    }
}
