using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    public PathCreator trainPath;
    [SerializeField] float trainSpeed;
    private float distanceTravelled;
    public bool isTrainMoving;
    public float rotationTracker;
    Vector3 trainRot;

    private void Start()
    {
        isTrainMoving = false;
        trainRot = new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        distanceTravelled += trainSpeed * Time.deltaTime;
        rotationTracker = trainRot.y;
        if (!isTrainMoving)
        {
            transform.position = trainPath.path.GetPointAtDistance(distanceTravelled);
            var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(trainRot));
            transform.rotation *= axisRemapRotation;
        }
        
    }
}
