using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CompassBar : MonoBehaviour
{
    public RectTransform compassBarTransform;
    public RectTransform objectiveMarker1Transform;
    public RectTransform objectiveMarker2Transform;
    public RectTransform objectiveMarker3Transform;
    public RectTransform northMarkerTransform;
    public RectTransform southMarkerTransform;

    public Transform cameraObjectTransform;
    public Transform objectiveObject1Transform;
    public Transform objectiveObject2Transform;
    public Transform objectiveObject3Transform;

    // Update is called once per frame
    void Update()
    {
        SetMarkerPosition(objectiveMarker1Transform, objectiveObject1Transform.position);
        SetMarkerPosition(objectiveMarker2Transform, objectiveObject2Transform.position);
        SetMarkerPosition(objectiveMarker3Transform, objectiveObject3Transform.position);
        SetMarkerPosition(northMarkerTransform, Vector3.forward * 1000);
        SetMarkerPosition(southMarkerTransform, Vector3.back * 1000);
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        Vector3 directionToTarget = worldPosition - cameraObjectTransform.position;
        float angle = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z), new Vector2(cameraObjectTransform.transform.forward.x, cameraObjectTransform.transform.forward.z));
        float compassPositionX = Mathf.Clamp(2 * angle / Camera.main.fieldOfView, -1, 1);
        markerTransform.anchoredPosition = new Vector2(compassBarTransform.rect.width / 2 * compassPositionX, 0);
    }
}
