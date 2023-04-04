using HeathenEngineering.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawFakeCar : MonoBehaviour
{
    [SerializeField] GameObject fakeCar;
    [SerializeField] Transform carBody;
    [Viewable][SerializeField] float offsetonZ =1.85f;
    private Vector3 carBodyPosition;

    public void InstantiateNewCar()
    {
        carBodyPosition = new Vector3(carBody.position.x, carBody.position.y, carBody.position.z + offsetonZ);
        Instantiate(fakeCar, carBodyPosition, carBody.rotation);
    }
}
