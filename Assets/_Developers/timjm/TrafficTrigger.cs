using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficTrigger : MonoBehaviour
{
    public GameObject Target;
    public float speed = 1.0f;
    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, step);
        if(transform.position == Target.transform.position)
        {
            Target.GetComponent<TrafficDirector>().Car = this.gameObject;
            Target.GetComponent<TrafficDirector>().Lane();
        }
    }
}
