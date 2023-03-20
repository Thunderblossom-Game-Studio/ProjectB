using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrateMechanic : MonoBehaviour
{
    [SerializeField] Transform wayPoint1;
    [SerializeField] Transform wayPoint2;
    [SerializeField] Transform platForm;
    [SerializeField] float timeWait;
    [SerializeField] float timeDuration;

    private bool isMoving;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerWeaponController>() != null)
        {
            if (!isMoving)
            {
                StartCoroutine(MoveOverTime(wayPoint2, timeDuration));
                isMoving = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerWeaponController>() != null)
        {
            if (isMoving)
            {
                StartCoroutine(MoveOverTime(wayPoint1, timeDuration));
                isMoving = !isMoving;
            }
        }
    }

    //IEnumerator MovePlatformUp()
    //{
    //    yield return new WaitForSeconds(timeWait);
    //    Debug.Log("First Courotine started");
    //    while (platForm.position.y != wayPoint2.position.y)
    //    {
    //        platForm.position = Vector3.MoveTowards(platForm.position, wayPoint2.position, speed * Time.deltaTime);
    //        transform.position = platForm.position;
    //        yield return null;
    //    }
    //}

    //IEnumerator MovePlatformDown()
    //{
    //    yield return new WaitForSeconds(timeWait);
    //    Debug.Log("Second Courotine started");
    //    while (platForm.position.y != wayPoint1.position.y)
    //    {
    //        platForm.position = Vector3.MoveTowards(platForm.position, wayPoint1.position, speed * Time.deltaTime);
    //        transform.position = platForm.position;
    //        Debug.Log(" Second Position are not the same");
    //        yield return null;
    //    }
    //}

    IEnumerator MoveOverTime(Transform destination, float duration)
    {
        yield return new WaitForSeconds(timeWait);

        Vector3 startPosition = platForm.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            platForm.position = Vector3.Lerp(startPosition, destination.position, t);
            transform.position = platForm.position;
            yield return null;
        }
    }
}
