using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Yeeuth : MonoBehaviour
{

    public bool Yeet;
    public float Thrusty;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Yeet == true)
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * Thrusty);
        }


    }
}
