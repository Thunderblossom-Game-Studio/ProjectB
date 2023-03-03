using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerSpeedController : MonoBehaviour
{
    [SerializeField] private bool debugSpeedAtRunTime;
    [SerializeField] private float speed;
    [SerializeField] private List<PropellerMovement> speedContainer;  
    // Start is called before the first frame update
    void Start()
    {
        SetAllSpeed();
    }

    private void Update()
    {
        if(debugSpeedAtRunTime)
        {
            SetAllSpeed();
        }
    }

    private void SetAllSpeed()
    {
        foreach (PropellerMovement propellerSpeed in speedContainer)
        {
             propellerSpeed.ChangeSpeed(speed);
        }
    }
}
