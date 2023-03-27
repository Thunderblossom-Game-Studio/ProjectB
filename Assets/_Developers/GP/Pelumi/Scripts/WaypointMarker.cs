using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaypointMarker : MonoBehaviour
{
    public static WaypointMarker Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI distanceDisplay;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 textOffset;
    [SerializeField] private Transform target;
    public IconType IconType;
    [SerializeField] private Sprite[] Markers;
    [SerializeField] private Image marker;
    [Viewable] [SerializeField] private CanvasGroup canvasGroup;

    private Camera mainCam;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
       
        canvasGroup = GetComponent<CanvasGroup>();
        mainCam = Camera.main;
    }


    void Update()
    {
        if (target == null)
        {
            canvasGroup.alpha = 0;
            return;
        }
        

        float minX = marker.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = marker.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = mainCam.WorldToScreenPoint(target.position + offset);

        if (Vector3.Dot((target.position - mainCam.transform.position), mainCam.transform.forward) < 0)
        {
            pos.x = minX;
            pos.x = pos.x < Screen.width / 2 ? maxX : minX;
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
        distanceDisplay.text = ((int)Vector3.Distance(target.position, mainCam.transform.position)).ToString() + "M";
    }

    public void SetIcon(IconType IconType)
    {
        switch (IconType)
        {
            case IconType.Package:
                marker.sprite = Markers[0];
                break;
            case IconType.Delivery:
                marker.sprite = Markers[1];
                break;
            
        }
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        canvasGroup.alpha = 1;
    }

}
public enum IconType
{

    Package,Delivery
}

