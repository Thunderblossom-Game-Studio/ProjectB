using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControl : MonoBehaviour
{
    public GameObject Traffic;
    private GameObject SpawnerReference;
    GameObject Clone;
    public GameObject RoadConnect;
    public int limit;
    public int count;
    bool ready = true;

    private void Awake()
    {
        SpawnerReference = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if ((ready == true)&&(count < limit))
        {
            StartCoroutine(Spawn());
            ready = false;
        }
    }

    IEnumerator Spawn()
    {
        Clone = Instantiate(Traffic, transform.position, Quaternion.identity);
        Clone.GetComponent<PassOn>().connect = RoadConnect.transform;       
        Clone.GetComponent<PassOn>().Controller = SpawnerReference;
        Clone.GetComponent<PassOn>().Pass(); 

        count += 1;
        yield return new WaitForSeconds(3);
        ready = true;
    }
}
