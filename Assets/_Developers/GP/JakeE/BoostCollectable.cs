using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostCollectable : Collectable
{
    [SerializeField] private uint _increasedBoost;
    
    protected override void Collect(GameObject collideObject)
    {
        VechicleResources vechicleResources = collideObject.GetComponent<VechicleResources>();
        if (vechicleResources == null) return;
        vechicleResources.IncreaseResource("Fuel", _increasedBoost);
    }
}
