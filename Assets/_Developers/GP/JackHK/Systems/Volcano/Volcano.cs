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
    public float waitTime;
    public float attackRate;

    private float timer;
    private Vector3 tempTarget;
        
    private void Update()
    {
        if (target == null) return;
        if (timer > attackRate)
        {
            StartCoroutine(FireVolcano());
            timer = 0;
        }
        else { timer += Time.deltaTime; }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    private IEnumerator FireVolcano()
    {
        Warn();
        yield return new WaitForSeconds(waitTime);
        Attack();
    }
    
    private void Warn()
    {
        if (target == null) return;
        tempTarget = RandomVector(target.position);
        Instantiate(warning, tempTarget, Quaternion.identity);
    }

    private void Attack()
    {
        GameObject proj = Instantiate(projectile, origin.position, Quaternion.identity);
        StartCoroutine(Curve.TransformCurve(proj, speed, angle, origin.position, tempTarget));
    }

    private Vector3 RandomVector(Vector3 vector)
    {
        vector += new Vector3(UnityEngine.Random.Range(-randomDeviation, randomDeviation), 0, UnityEngine.Random.Range(-randomDeviation, randomDeviation));
        return vector;
    }
}
