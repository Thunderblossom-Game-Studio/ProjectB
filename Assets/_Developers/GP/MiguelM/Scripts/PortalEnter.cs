using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class PortalEnter : MonoBehaviour
{
    public Transform carTransform;
    public List<Transform> ExitPortal;

    public bool randomPosActive;
    public SendPlayerToRandomPos PlayerRandomPosScript;

    int rand;
    public float timeToTravel;
    public float playerOffseat;
    // Start is called before the first frame update
    void Start()
    {
        timeToTravel = 5.0f;
        rand = 0;
        playerOffseat = 0.0f;

        if (randomPosActive == true)
        {
            PlayerRandomPosScript.enabled = true;
            Debug.Log("Script Previously Active");
        }
        //else
        //{
        //    randomPosActive = false;
        //    Debug.Log("Script Previously Not Active");
        //}
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
            //rand = UnityEngine.Random.Range(0, ExitPortal.Count);
            timeToTravel -= Time.deltaTime;
        }

        if (randomPosActive == true)
        {
            PlayerRandomPosScript.enabled = true;
            Debug.Log("Script Still Active");
        }
        else
        {
            randomPosActive = false;
            PlayerRandomPosScript.enabled = false;
            Debug.Log("Script Still Not Active");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // Do a courotine first
        if(other.CompareTag("Player"))
        {
            //carTransform.rotation = Quaternion.Euler(Quaternion.identity.x, ExitPortal[0].transform.rotation.y, Quaternion.identity.z);
            //carTransform.rotation = ExitPortal[0].transform.rotation;
            carTransform.rotation = Quaternion.Euler(0, -ExitPortal[0].transform.rotation.y, 0);
            carTransform.position = ExitPortal[0].position;
        }
    }
}
