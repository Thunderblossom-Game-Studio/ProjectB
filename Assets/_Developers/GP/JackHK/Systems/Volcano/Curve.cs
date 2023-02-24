using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Curve
{
    public static IEnumerator TransformCurve(GameObject projectile, float speed, float angle, Vector3 startPoint, Vector3 endPoint)
    {
        float count = 0.0f;
        Vector3 midPoint = startPoint + (endPoint + -startPoint) / 2 + Vector3.up * angle;
        Debug.Log("curve");

        while (count < 1.0f)
        {
            count += speed * Time.deltaTime;
            Vector3 A = Vector3.Lerp(startPoint, midPoint, count);
            Vector3 B = Vector3.Lerp(midPoint, endPoint, count);
            projectile.transform.position = Vector3.Lerp(A, B, count);
            
            yield return null;
        }
    }
}
