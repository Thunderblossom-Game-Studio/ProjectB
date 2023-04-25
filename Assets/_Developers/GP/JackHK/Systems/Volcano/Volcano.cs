using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    private VolcanoDetector detector;
    private float timer;
    private Vector3 tempTarget;
    private float distance;
    public float timeToReachTarget;

    [Tooltip("The projectile that the volcano will shoot")]
    public GameObject projectile;

    [Tooltip("The warning displayed before projectile hits target")]
    public GameObject warning;

    [Tooltip("Where the projectile fires from")]
    public Transform origin;

    [Tooltip("Does this volcano target the player's position or a random spot?")]
    public bool targetsPlayer;

    [Tooltip("Speed at which the projectile travels")]
    public float speed;

    [Tooltip("Randomness in deviation where the projectile will land")]
    public float randomDeviation;

    [Tooltip("Vertical angle of the projectile path")]
    public float angle;

    [Tooltip("Attack fire rate in seconds")]
    public float attackRate;

    [Tooltip("Ignore this. Used for debugging only")]
    public float debugTargetModifier;

    // The target of the volcano.
    [HideInInspector] public Transform target;

    private void Start()
    {
        detector = GetComponentInParent<VolcanoDetector>();
    }

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
        tempTarget = RandomVector(target.transform.position);
        GameObject proj = Instantiate(projectile, origin.position, Quaternion.identity);
        StartCoroutine(Curve(proj, speed, angle, origin.position, tempTarget));
    }

    public Vector3 RandomVector(Vector3 vector)
    {
        vector += new Vector3(UnityEngine.Random.Range(-randomDeviation, randomDeviation), 0, UnityEngine.Random.Range(-randomDeviation, randomDeviation));
        return vector;
    }

    private IEnumerator Curve(GameObject projectile, float speed, float angle, Vector3 startPoint, Vector3 endPoint)
    {
        float count = 0.0f;
        Vector3 midPoint = startPoint + (endPoint + -startPoint) / 2 + Vector3.up * angle;
        distance = Vector3.Distance(startPoint, endPoint);
        timeToReachTarget = (distance / speed);
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
