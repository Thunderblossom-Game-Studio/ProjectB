using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMarkerReset : MonoBehaviour
{
    public void ResetWaypointMarker()
    {
        WaypointMarker.Instance.SetTarget(null);
    }
}
