using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    public GameObject projectile;
    public GameObject warning;
    public Transform origin;
    public Transform target;
    public float speed;
    public float randomDeviation;
    public float angle;
    public float attackRate;

    private float timer;
    private Vector3 tempTarget;
    private float distance;
    public float timeToReachTarget;

    private void Update()
    {
        if (target == null) return;
        if (timer > attackRate)
        {
            Attack();
            timer = 0;
        }
        else { timer += Time.deltaTime; }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Warn()
    {
        GameObject warn = Instantiate(warning, tempTarget, Quaternion.identity);
        warn.GetComponent<Indicator>().UpdateStuff(timeToReachTarget);
    }

    private void Attack()
    {
        if (target == null) return;
        tempTarget = RandomVector(target.position);
        GameObject proj = Instantiate(projectile, origin.position, Quaternion.identity);
        StartCoroutine(Curve(proj, speed, angle, origin.position, tempTarget));
    }

    private Vector3 RandomVector(Vector3 vector)
    {
        vector += new Vector3(UnityEngine.Random.Range(-randomDeviation, randomDeviation), 0, UnityEngine.Random.Range(-randomDeviation, randomDeviation));
        return vector;
    }

    private IEnumerator Curve(GameObject projectile, float speed, float angle, Vector3 startPoint, Vector3 endPoint)
    {
        float count = 0.0f;
        Vector3 midPoint = startPoint + (endPoint + -startPoint) / 2 + Vector3.up * angle;
        Debug.Log("curve");

        distance = Vector3.Distance(startPoint, endPoint);
        timeToReachTarget = (distance / speed);
        Debug.Log(timeToReachTarget);
        Warn();

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
