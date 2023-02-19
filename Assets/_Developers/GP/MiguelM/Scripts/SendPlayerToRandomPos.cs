using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SendPlayerToRandomPos : MonoBehaviour
{
    float carX = 0;
    float carY = 0;
    float carZ = 0;
    public float Timer = 10f;
    public bool isScriptActive = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!isScriptActive)
            isScriptActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        carX = Random.Range(-252.87f, 242.69f);
        carY = Random.Range(-2.47f, 15.77f);
        carZ = Random.Range(-239.85f, 252.19f);

        if(isScriptActive)
        {
            Timer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Road") || other.CompareTag("Ground"))
        {
            if (Timer <= 1.0f)
            {
                transform.position = new Vector3(carX, carY, carZ);
                transform.rotation = Quaternion.identity;
                Debug.Log("Collision Detected!");
                Timer = 10.0f;
            }
        }
    }
    //private void OnTriggerStay(Collision collision)
    //{
        
    //}

 
}
