using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private List<RouteUser> routeUsers;

    public void Start()
    {
        StartCoroutine(ActivateTrain());
    }

    public IEnumerator ActivateTrain()
    {
        for (int i = 0; i < routeUsers.Count; i++)
        {
            routeUsers[i].Activate();
            yield return new WaitForSeconds(delay);
        }
    }
}
