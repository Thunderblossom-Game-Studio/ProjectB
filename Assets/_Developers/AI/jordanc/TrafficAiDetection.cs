using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficAiDetection : MonoBehaviour
{

    public GameObject TrafficCarObject;
    public bool stop;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        TrafficCarObject.GetComponent<TrafficBrain>().agent.isStopped = true;
        stop = true;

    }

    private void OnTriggerExit(Collider other)
    {
        TrafficCarObject.GetComponent<TrafficBrain>().agent.isStopped = false;
        stop = false;
    }


    private void OnTriggerStay(Collider other)
    {
        TrafficCarObject.GetComponent<TrafficBrain>().agent.isStopped = true;
        stop = true;
        
    }


    

}
