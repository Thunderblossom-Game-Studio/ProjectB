using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathUtil
{
    public static void DrawPath(LineRenderer lineRenderer, Vector3 startPos, Vector3 endPos, float angle, int vertexCount)
    {
        Vector3 midPoint = startPos + (endPos - startPos) / 2 + Vector3.up * angle;
        Vector3[] positions = new Vector3[vertexCount];
        for (int i = 0; i < vertexCount; i++)
        {
            float t = (float)i / (vertexCount - 1);
            positions[i] = CalculateCurvePosition(startPos, midPoint, endPos, t);
        }

        lineRenderer.positionCount = vertexCount;
        lineRenderer.SetPositions(positions);
    }

    public static Vector3 CalculateCurvePosition(Vector3 start, Vector3 mid, Vector3 end, float t)
    {
        float oneMinusT = 1f - t;
        return (oneMinusT * oneMinusT * start) + (2f * oneMinusT * t * mid) + (t * t * end);
    }

    public static float CalculateAngle(Vector3 start, Vector3 end, float minAngle, float maxAngle)
    {
        Vector3 direction = end - start;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < minAngle) angle = minAngle; else if (angle > maxAngle) angle = maxAngle; return angle;
    }

    public static IEnumerator DoMove(Transform objectToMove, Vector3 startPos, Vector3 endPos, float angle, float speed, Action OnFinished = null)
    {
        float dist = Vector3.Distance(startPos, endPos);
        float duration = dist / speed;
        float t = 0.0f;
        Vector3 midPoint = startPos + (endPos - startPos) / 2 + Vector3.up * angle;

        while (t < duration)
        {
            t += Time.deltaTime;
            float frac = t / duration;
            Vector3 start = Vector3.Lerp(startPos, midPoint, frac);
            Vector3 end = Vector3.Lerp(midPoint, endPos, frac);
            objectToMove.position = Vector3.Lerp(start, end, frac);
            yield return null;
        }

        objectToMove.position = endPos;

        if (OnFinished != null) OnFinished();
    }

    public static IEnumerator DoMove(Transform objectToMove, Vector3 startPos, Vector3 endPos, float angle, Action OnFinished = null)
    {
        float count = 0.0f;
        Vector3 midPoint = startPos + (endPos + -startPos) / 2 + Vector3.up * angle;
        while (count < 1.0f)
        {
            count += 1.0f * Time.deltaTime;
            Vector3 start = Vector3.Lerp(startPos, midPoint, count);
            Vector3 end = Vector3.Lerp(midPoint, endPos, count);
            objectToMove.position = Vector3.Lerp(start, end, count);
            yield return null;
        }

        if (OnFinished != null) OnFinished();
    }

    public static IEnumerator MoveRigidbodyAlongLine(Rigidbody objectToMove, Vector3 startPos, Vector3 endPos, float angle, float speed, Action OnFinished = null)
    {
        Vector3 midPoint = startPos + (endPos - startPos) / 2 + Vector3.up * angle;
        float distance = Vector3.Distance(startPos, endPos);
        float timeToMove = distance / speed;
        float t = 0f;

        while (t < timeToMove)
        {
            t += Time.deltaTime;
            float fraction = t / timeToMove;
            Vector3 newPos = CalculateCurvePosition(startPos, midPoint, endPos, fraction);
            objectToMove.MovePosition(newPos);
            yield return null;

            // Check if object hit the end position
            if (Vector3.Distance(newPos, endPos) < 0.01f)
            {
                Vector3 pathDirection = (endPos - startPos).normalized;
                objectToMove.AddForce(pathDirection * speed, ForceMode.Impulse);
                objectToMove.isKinematic = false;
                OnFinished?.Invoke();
                yield break;
            }
        }

        // If the coroutine reaches here, it means the object reached the end position
        objectToMove.GetComponent<Rigidbody>().isKinematic = false;
    }

    public static IEnumerator MoveObjectAlongPath(Transform objectToMove, Vector3 startPos, Vector3 endPos, float angle, float speed, Action OnReachTraget = null)
    {
        float dist = Vector3.Distance(startPos, endPos);
        float duration = dist / speed;
        float t = 0.0f;
        Vector3 midPoint = startPos + (endPos - startPos) / 2 + Vector3.up * angle;

        while (t < duration)
        {
            t += Time.deltaTime;
            float frac = t / duration;
            Vector3 start = Vector3.Lerp(startPos, midPoint, frac);
            Vector3 end = Vector3.Lerp(midPoint, endPos, frac);
            objectToMove.position = Vector3.Lerp(start, end, frac);
            objectToMove.rotation = Quaternion.LookRotation(end - start, Vector3.up);
            yield return null;
        }
        if (OnReachTraget != null) OnReachTraget();
        objectToMove.position = endPos;

        // Keep moving the object in the same direction with the same speed
        Vector3 direction = (endPos - startPos).normalized;
        while (true)
        {
            objectToMove.position += objectToMove.forward * speed * Time.deltaTime;
            yield return null;
        }
    }
}
