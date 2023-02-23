using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    [Header("Turret gameobjects")]
    public Transform turretBase;
    public Transform turretBarrel;

    [Header("Rotation settings")]
    [Range(0, 180)]
    public float rightRotationLimit;
    [Range(0, 180)]
    public float leftRotationLimit;
    [Range(0, 90)]
    public float elevationRotationLimit;
    [Range(0, 90)]
    public float depressionRotationLimit;
    [Range(0, 300)]
    public float TurnSpeed;

    private Vector3 aimPoint;

    public void SetAim(Vector3 AimPosition)
    {
        aimPoint = AimPosition;
    }

    private void Update()
    {
        aimPoint = Camera.main.transform.forward * 200.0f;
        HorizontalRotation();
        VerticalRotation();
    }

    private void HorizontalRotation()
    {
        // Get aim position in parent gameobject local space in relation to aim position world space 
        Vector3 targetPositionInLocalSpace = transform.InverseTransformPoint(aimPoint);
        // Set "aimPoint" Y position to zero, since this is horizontal rotation n because we dont need it
        targetPositionInLocalSpace.y = 0.0f;
        // Store limit value of the rotation
        Vector3 limitedRotation = targetPositionInLocalSpace;

        // limit turret horizontal rotation according to its rotation limit
        if (targetPositionInLocalSpace.x >= 0.0f)
            limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, Mathf.Deg2Rad * rightRotationLimit, float.MaxValue);
        else
            limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, Mathf.Deg2Rad * leftRotationLimit, float.MaxValue);

        //Get direction
        Quaternion whereToRotate = Quaternion.LookRotation(limitedRotation);
        // Rotate the turret
        turretBase.localRotation = Quaternion.RotateTowards(turretBase.localRotation, whereToRotate, TurnSpeed * Time.deltaTime);
    }

    private void VerticalRotation()
    {
        // Get aim position in barrel gameobject local space in relation to aim position world space 
        Vector3 targetPositionInLocalSpace = turretBase.InverseTransformPoint(aimPoint);

        // Set "TargetPositionInLocalSpace" X position to zero, since this is vertical rotation n because we dont need it
        targetPositionInLocalSpace.x = 0.0f;
        // Store limit value of the rotation
        Vector3 limitedRotation = targetPositionInLocalSpace;
        // limit turret vertical rotation according to its rotation limit
        if (targetPositionInLocalSpace.y >= 0.0f)
            limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, Mathf.Deg2Rad * elevationRotationLimit, float.MaxValue);
        else
            limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, Mathf.Deg2Rad * depressionRotationLimit, float.MaxValue);

        //Get direction
        Quaternion whereToRotate = Quaternion.LookRotation(limitedRotation);
        // Rotate the barrel
        turretBarrel.localRotation = Quaternion.RotateTowards(turretBarrel.localRotation, whereToRotate, TurnSpeed * Time.deltaTime);

    }

    private void OnDrawGizmos()
    {

        // Draw a line to the aim point
        if (turretBarrel != null)
            Gizmos.DrawLine(turretBarrel.position, turretBarrel.forward * 200.0f);
    }
}
