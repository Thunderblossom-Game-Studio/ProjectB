using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TrainHead : MonoBehaviour
{
    [SerializeField] private List<RouteUser> trainParts;

    private void Start()
    {
        //for (int i = 0; i < trainParts.Count; i++) trainParts[i].Activate(0);

        StartCoroutine(ActivateTrain());
    }

    public IEnumerator ActivateTrain()
    {
        yield return new WaitForSecondsRealtime(.1f);
        for (int i = 0; i < trainParts.Count; i++)
        {
            trainParts[i].Activate(0);
            yield return null;
        }
    }
}
