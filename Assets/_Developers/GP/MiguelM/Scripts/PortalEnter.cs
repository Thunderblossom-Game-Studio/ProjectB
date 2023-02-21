using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class PortalEnter : MonoBehaviour
{
    public Transform carTransform;
    public List<Transform> ExitPortal;

    int rand;
    public float timeToTravel;
    // Start is called before the first frame update
    void Start()
    {
        timeToTravel = 5.0f;
        rand = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToTravel <= 1)
        {
            timeToTravel = 5.0f;
        }
        else
        {
            rand = UnityEngine.Random.Range(0, ExitPortal.Count);
            timeToTravel -= Time.deltaTime;
        }
           
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            carTransform.position = ExitPortal[rand].position;
        }
    }
}
