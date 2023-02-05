using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPrevention : MonoBehaviour
{

    public bool HitDetection;
    public static bool CarCollisionPrevention;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        CarCollisionPrevention = HitDetection;
    }


    private void OnTriggerEnter(Collider other)
    {
        HitDetection = true;
    }

    private void OnTriggerExit(Collider other)
    {
        HitDetection = false;
    }

}
