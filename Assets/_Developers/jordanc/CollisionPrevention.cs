using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPrevention : AITestCar
{

    public bool HitDetection;
    public static bool CarCollisionPrevention;


    public bool RayCastFireOne;
    public bool RayCastFireTwo;
    public LayerMask RayCastLayer;
    [SerializeField] GameObject LeftRayCast;
    [SerializeField] GameObject RightRayCast;
    public bool TurnLeft;
    public bool TurnLeftBoolPass;
    public bool TurnRight;
    public bool TurnRightBoolPass;
    public bool Brake;
    public bool BrakeBoolPass;

    private AITestCar Car;

    // Start is called before the first frame update
    void Start()
    {
        Car= GetComponent<AITestCar>();
    }

    // Update is called once per frame
    private void Update()
    {
        CarCollisionPrevention = HitDetection;
        RaycastCheck();

        TurnLeftBoolPass = TurnLeft;
        TurnRightBoolPass = TurnRight;
        BrakeBoolPass= Brake;

        if (RayCastFireOne == true && RayCastFireTwo == false)
        {
            TurnLeft = false;
            //Car.Turn(-1);
           // Car.Accelerate(1);
            TurnRight = true;
            Brake = false;
        }

        else if(RayCastFireOne == false && RayCastFireTwo == true)
        {
            TurnRight = false;
            //Car.Turn(1);
            //Car.Accelerate(1);
            TurnLeft = true;
            Brake = false;
        }

        else if (RayCastFireOne == true && RayCastFireTwo == true)
        {
            Brake = true;
            //Car.Accelerate(0);
            TurnRight = false;
            TurnLeft = false;
        }
        
        else
        {
            TurnLeft = false;
            TurnRight = false;
            Brake = false;
        }



    }


    void RaycastCheck()
    {
        RayCastFireOne = Physics.Raycast(LeftRayCast.transform.position, transform.forward, 5, RayCastLayer);
        RayCastFireTwo = Physics.Raycast(RightRayCast.transform.position, transform.forward, 5, RayCastLayer);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(LeftRayCast.transform.position, transform.forward * 5);
        
        Gizmos.DrawRay(RightRayCast.transform.position, transform.forward * 5);
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
