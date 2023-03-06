using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionWaypointSystem : MonoBehaviour
{

    public Vector3 offset;
    public Vector3 textOffset;

    public List<Image> markers;

    public List<Image> images;
    public List<Transform> targets;
    public List<Text> meters;

    public float rotOffset = -90f;
    public float rotMult = 10f;

    void Update()
    {


        for (int i = 0; i < targets.Count; i++) 
        {
            float minX = markers[i].GetPixelAdjustedRect().width / 2;
            float maxX = Screen.width - minX;

            float minY = markers[i].GetPixelAdjustedRect().height / 2;
            float maxY = Screen.height - minY;

            Vector2 pos = Camera.main.WorldToScreenPoint(targets[i].position + offset);


            float rotshit = 0f;

            if (Vector3.Dot((targets[i].position - transform.position), transform.forward) < 0)
            {
                // target is behind the player
                if (pos.x < Screen.width / 2)
                {
                    pos.x = maxX;

                    rotshit = -180f;
                }
                else
                {
                    pos.x = minX;
                }
            }

            Vector3 adjustedTarget = new Vector3(targets[i].transform.position.x, targets[i].transform.position.z, transform.position.z);
            Vector3 adjustedCam = new Vector3(transform.position.x, transform.position.z, transform.position.z);

            Vector3 vectorToTarget = adjustedTarget - adjustedCam;
            vectorToTarget = vectorToTarget.normalized * rotOffset;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle-rotOffset + rotshit, Vector3.forward);
            images[i].transform.rotation = Quaternion.Slerp(images[i].transform.rotation, q, Time.deltaTime * 2);


            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);


        

            markers[i].transform.position = pos;

        
            meters[i].text = ((int)Vector3.Distance(targets[i].position, transform.position)).ToString() + "M";
        }

    }
}
