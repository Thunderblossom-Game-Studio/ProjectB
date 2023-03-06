using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTipText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 spawnPosition;

    public string tipToShow;
    private float timeTowait = 0.9f;
    public void OnPointerEnter(PointerEventData eventData)
    {
         
        StopAllCoroutines();
        StartCoroutine(StartTimer());
        spawnPosition = eventData.pointerEnter.transform.position;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
        StopAllCoroutines();
        HoverManager.OnMouseLoseFocus();

    }

    private void ShowMessage()
    {
        HoverManager.OnMouseHover(tipToShow, spawnPosition);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeTowait);
        ShowMessage();

    }
}
