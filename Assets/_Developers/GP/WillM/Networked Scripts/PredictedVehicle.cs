using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictedVehicle : NetworkBehaviour
{
    // Holds visuals
    [SerializeField] private Transform visualRootObject;
    // Holds non-visual elements
    [SerializeField] private GameObject scriptRootObject;
    // Duration of smoothing
    private float smoothingDuration = 0.05f;

    #region Local State

    // Check if subscribed to Timer Manager events
    private bool isSubscribed = false;

    // Car controller (input)
    private NetworkedVehicleController _controller;

    

    #endregion
}
