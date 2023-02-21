//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class MissionWaypoint : MonoBehaviour
//{
//    //public Image img1;
//    //public Image img2;
//    //public Image img3;
//    //public Transform target1;
//    //public Transform target2;
//    //public Transform target3;
//    //public Text meter1;
//    //public Text meter2;
//    //public Text meter3;
//    public Vector3 offset;
//    public Vector3 textOffset;

//    public List<Image> markers;

//    public List<Image> images;
//    public List<Transform> targets;
//    public List<Text> meters;

//    public float rotOffset = -90f;
//    public float rotMult = 10f;

//    void Update()
//    {
//        //float minX1 = img1.GetPixelAdjustedRect().width / 2;
//        //float maxX1 = Screen.width - minX1;

//        //float minY1 = img1.GetPixelAdjustedRect().height / 2;
//        //float maxY1 = Screen.height - minY1;

//        //float minX2 = img2.GetPixelAdjustedRect().width / 2;
//        //float maxX2 = Screen.width - minX2;

//        //float minY2 = img2.GetPixelAdjustedRect().height / 2;
//        //float maxY2 = Screen.height - minY2;

//        //float minX3 = img3.GetPixelAdjustedRect().width / 2;
//        //float maxX3 = Screen.width - minX3;

//        //float minY3 = img3.GetPixelAdjustedRect().height / 2;
//        //float maxY3 = Screen.height - minY3;

//        for (int i = 0; i < targets.Count; i++) 
//        {
//            float minX = markers[i].GetPixelAdjustedRect().width / 2;
//            float maxX = Screen.width - minX;

//            float minY = markers[i].GetPixelAdjustedRect().height / 2;
//            float maxY = Screen.height - minY;

//            Vector2 pos = Camera.main.WorldToScreenPoint(targets[i].position + offset);


//            float rotshit = 0f;

//            if (Vector3.Dot((targets[i].position - transform.position), transform.forward) < 0)
//            {
//                // target is behind the player
//                if (pos.x < Screen.width / 2)
//                {
//                    pos.x = maxX;

//                    rotshit = -180f;
//                }
//                else
//                {
//                    pos.x = minX;
//                }
//            }

//            Vector3 adjustedTarget = new Vector3(targets[i].transform.position.x, targets[i].transform.position.z, transform.position.z);
//            Vector3 adjustedCam = new Vector3(transform.position.x, transform.position.z, transform.position.z);

//            Vector3 vectorToTarget = adjustedTarget - adjustedCam;
//            vectorToTarget = vectorToTarget.normalized * rotOffset;
//            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
//            Quaternion q = Quaternion.AngleAxis(angle-rotOffset + rotshit, Vector3.forward);
//            images[i].transform.rotation = Quaternion.Slerp(images[i].transform.rotation, q, Time.deltaTime * 2);


//            pos.x = Mathf.Clamp(pos.x, minX, maxX);
//            pos.y = Mathf.Clamp(pos.y, minY, maxY);


//            //pos.y = minY;

//            markers[i].transform.position = pos;

//            //Vector2 s = new Vector2((targets[i].position - transform.position).x, (targets[i].position - transform.position).z);

//            //images[i].transform.LookAt(s - (Vector2)transform.position, Vector2.up);

//            //meters[i].transform.position = images[i].transform.position + textOffset;
//            meters[i].text = ((int)Vector3.Distance(targets[i].position, transform.position)).ToString() + "M";
//        }

//        #region United Kingdom
//        /*


//        Vector2 pos1 = Camera.main.WorldToScreenPoint(target1.position + offset);

//        if(Vector3.Dot((target1.position - transform.position), transform.forward) <0 )
//        {
//            // target is behind the player
//            if(pos1.x < Screen.width / 2)
//            {
//                pos1.x = maxX1;
//            }
//            else
//            {
//                pos1.x = minX1;
//            }
//        }

//        pos1.x = Mathf.Clamp(pos1.x, minX1, maxX1);
//        pos1.y = Mathf.Clamp(pos1.y, minY1, maxY1);



//        Vector2 pos2 = Camera.main.WorldToScreenPoint(target2.position + offset);

//        if (Vector3.Dot((target1.position - transform.position), transform.forward) < 0)
//        {
//            // target is behind the player
//            if (pos2.x < Screen.width / 2)
//            {
//                pos2.x = maxX2;
//            }
//            else
//            {
//                pos2.x = minX2;
//            }
//        }

//        pos2.x = Mathf.Clamp(pos2.x, minX2, maxX2);
//        pos2.y = Mathf.Clamp(pos2.y, minY2, maxY2);

//        Vector2 pos3 = Camera.main.WorldToScreenPoint(target3.position + offset);

//        if (Vector3.Dot((target1.position - transform.position), transform.forward) < 0)
//        {
//            // target is behind the player
//            if (pos3.x < Screen.width / 2)
//            {
//                pos3.x = maxX3;
//            }
//            else
//            {
//                pos3.x = minX3;
//            }
//        }

//        pos3.x = Mathf.Clamp(pos3.x, minX3, maxX3);
//        pos3.y = Mathf.Clamp(pos3.y, minY3, maxY3);

//        img1.transform.position = pos1;
//        img2.transform.position = pos2;
//        img3.transform.position = pos3;
//        meter1.text = ((int)Vector3.Distance(target1.position, transform.position)).ToString() + "M";
//        meter2.text = ((int)Vector3.Distance(target2.position, transform.position)).ToString() + "M";
//        meter3.text = ((int)Vector3.Distance(target3.position, transform.position)).ToString() + "M";

//        */
//        #endregion
//    }
//}
